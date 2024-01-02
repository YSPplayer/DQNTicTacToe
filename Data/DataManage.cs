using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using TicTacToe.Enum;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace TicTacToe.Data
{
    public class Sample
    {
        public string S1 { get;private set; }
        public string Action { get; private set; }
        public int Value { get; private set; }
        public int ActionCount { get; private set; }
        public int End { get; private set; }
        public Sample(string s1, string action, int value, int actionCount, int end)
        {
            S1 = s1;
            Action = action;
            Value = value;
            ActionCount = actionCount;
            End = end;
        }
    }
	public class DataManage
	{

        private static Queue<Sample> sampleQueue = new Queue<Sample>();
        private static readonly object lockObject = new object();
        //数据库
        private static SQLiteConnection conn;
        public static void Start()
        {
            //开启db异步线程
            Task.Factory.StartNew(() => SQLiteStart());
        }

        /// <summary>
        /// 入队列
        /// </summary>
        private static void SampleEnqueue(Sample sample)
        {
            lock (lockObject)
            {
                sampleQueue.Enqueue(sample);
                Monitor.Pulse(lockObject);  // 通知等待的线程
            }
        }

        /// <summary>
        /// 出队列
        /// </summary>
        /// <param name="sample"></param>
        private static Sample SampleDequeue()
        {
            lock (lockObject)
            {
               return sampleQueue.Dequeue();
            }
        }

        private static void SQLiteStart()
        {
            // 数据库文件的路径
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SampleData");
            string dbPath = Path.Combine(folderPath, "sample.db");
            // 确保db文件夹存在
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            // 创建SQLite连接
            conn = new SQLiteConnection($"Data Source={dbPath};Version=3;");
            try
            {
                conn.Open(); //打开数据库连接
                // 创建表
                string executeCommand = @"
                CREATE TABLE IF NOT EXISTS sampleTable (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    S1 TEXT,
                    Action TEXT,
                    Value INTEGER,
                    ActionCount INTEGER,
                    End INTEGER
                );";
                SQLiteCommand createTableCmd = new SQLiteCommand(executeCommand, conn);
                createTableCmd.ExecuteNonQuery();//执行命令
                //List<Sample> samples = DataManage.SearchData();
                while (true)
                {
                    lock (lockObject)
                    {
                        while (sampleQueue.Count == 0)
                        {
                            Monitor.Wait(lockObject); // 等待队列中有元素
                        }  
                    }
                    Sample sample = SampleDequeue();
                    InsertIntoData(sample);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally 
            {
                // 关闭数据库连接
                conn.Close();
            }
        }

        /// <summary>
        /// 插入数据到数据库
        /// </summary>
        /// <param name="item"></param>
        private static void InsertIntoData(Sample sample)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(conn))
            {
                cmd.CommandText = "INSERT INTO sampleTable (S1, Action, Value, ActionCount, End) VALUES (@s1, @action, @value, @actionCount, @end)";
                cmd.Parameters.AddWithValue("@s1", sample.S1);
                cmd.Parameters.AddWithValue("@action", sample.Action);
                cmd.Parameters.AddWithValue("@value", sample.Value);
                cmd.Parameters.AddWithValue("@actionCount", sample.ActionCount);
                cmd.Parameters.AddWithValue("@end", sample.End);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 获取到我们已经存储的样本数据
        /// </summary>
        private static List<Sample> SearchData()
        {
            List<Sample> samples = new List<Sample>();
            using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM sampleTable", conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Sample newSample = new Sample(reader["S1"].ToString(), reader["Action"].ToString(), Convert.ToInt32(reader["Value"]),
                            Convert.ToInt32(reader["ActionCount"]), Convert.ToInt32(reader["End"]));
                        samples.Add(newSample);
                    }
                }
            }
            return samples;
        }
        public static void SavaData(int[,] outs, Location outDropLocation, int outValue,int actionCount,bool end)
		{ 
            string s1 = ArrayToString(outs);
            string action = $"{outDropLocation.x},{outDropLocation.y}";
            int value = outValue;
            int iend = end ? 1 : 0;
            SampleEnqueue(new Sample(s1, action, value, actionCount, iend));
        }

        /// <summary>
        /// 多维数组转成string存储
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        private static string ArrayToString(int[,] array)
        {
            List<string> elements = new List<string>();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    elements.Add(array[i, j].ToString());
                }
            }
            return string.Join(",", elements);
        }
    }
}

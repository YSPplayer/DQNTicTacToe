using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TicTacToe.DQN
{
    class ScriptManage
    {
        private static Socket clientSocket;//客户端的socket
        private static string serverIp = "127.0.0.1";
        private static int port = 7913;
        private const int MAX_BUFFER = 1024;
        private static byte[] receiveBuffer = new byte[MAX_BUFFER];
        private static string ConvertArrayToJson(int[,] array)
        {
            return JsonConvert.SerializeObject(array);
        }

        private static string ConvertListToJson(List<int[]> list)
        {
            return JsonConvert.SerializeObject(list);
        }

        private static void ConnectServer()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress serverIP = IPAddress.Parse(serverIp);
            // 连接服务器
            clientSocket.Connect(new IPEndPoint(serverIP, port));
        }

        /// <summary>
        /// 读取脚本效率太低，把python写成服务器程序
        /// </summary>
        public static bool StartModelServer()
        {
            //启动脚本，开启服务器
            //获取模型的文件路径
            //string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Model");
            //string modelPath = Path.Combine(folderPath, "dqn_model.h5");
            string modelPath = "D:\\ZfraTemp\\DQNTicTacToe\\bin\\Debug\\Model\\dqn_model.h5";
            // if (!RunPythonScriptServer(modelPath)) return false;
            if (!WaitForServerStartup(TimeSpan.FromSeconds(6))) return false;
           // ConnectServer();
            //SendMessageSync("hello world");
            //SendMessageSync("CLOSE");
            //clientSocket.Close();
            //return true;
            return true;
        }

        public static void Close()
        {
            SendMessageSync("CLOSE");
            clientSocket.Close();
        }

        public static int[] DropPiece(int[,] s, List<int[]> actions)
        {
            if (!clientSocket.Connected) return new int[] { -1, -1 };
            var matrixJson = ConvertArrayToJson(s);
            var listJson = ConvertListToJson(actions);
            SendMessageSync("A" + matrixJson);
            SendMessageSync("B" + listJson);
            string result = ReceiveMessageSync();
            string[] resultArray = result.Split(',');
            return new int[] { int.Parse(resultArray[0]), int.Parse(resultArray[1]) };
        }
        public static string ReceiveMessageSync()
        {
            byte[] buffer = new byte[MAX_BUFFER];
            int len = -1;
            try
            {
                len = clientSocket.Receive(buffer);
            }
            catch (Exception e)
            {
                clientSocket.Close();
                return null;
            }
            if (len <= 0)
            {
                return null;
            }
            else
            {
                return Encoding.UTF8.GetString(buffer, 0, len);
            }

        }

        public static void ReceiveMessage()
        {
            try
            {
                //开始异步线程接收
                clientSocket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                clientSocket.Close();
                return;
            }
        }
        private static void SendCallback(IAsyncResult result)
        {
            try
            {
                //执行回调函数，这个是返回发送的字节数
                int bytesSent = clientSocket.EndSend(result);
            }
            catch (Exception ex)
            {
            }
        }
        private static void ReceiveCallback(IAsyncResult ar)
        {
            // 完成异步数据接收，获取数组 需要try
            int bytesRead = clientSocket.EndReceive(ar);
            if (bytesRead > 0)
            {
                // 处理接收到的数据
                string receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);
                // 继续异步接收数据
                ReceiveMessage();
            }
        }
        public static int SendMessageSync(string message)
        {
            //发送信息给服务器
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            try
            {
                return clientSocket.Send(buffer);
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        private static bool WaitForServerStartup(TimeSpan timeout)
        {
            DateTime deadline = DateTime.Now + timeout;
            while (DateTime.Now < deadline)
            {
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    client.Connect(new IPEndPoint(IPAddress.Parse(serverIp), port));
                    client.Send(Encoding.UTF8.GetBytes("PING"));
                    clientSocket = client;
                    return true; // 连接成功，服务器已启动
                }
                catch (SocketException e)
                {
                    client.Close();
                    // 连接失败，等待后重试
                    Thread.Sleep(1000); // 等待1秒
                }
                
            }
            return false; // 超时，服务器未启动
        }

        private static bool RunPythonScriptServer(string path)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Script");
            string pyPath = Path.Combine(folderPath, "modelAiServer.py");
            start.FileName = "D:\\soft\\Anaconda\\python.exe"; // 指定 python 解释器的路径
            //D:\\soft\\Anaconda\\python.exe D:\\anaconda\\python.exe
            start.Arguments = $"{pyPath} \"{path}\"";// 指定脚本文件，参数为空
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true; // 重定向标准错误流
            start.CreateNoWindow = true;
            Process process = Process.Start(start);
            process.Close();
            return true;
        }

        public static int[] RunPythonScript(int[,] s, List<int[]> actions)
        {
            var matrixJson = ConvertArrayToJson(s);
            var listJson = ConvertListToJson(actions);
            ProcessStartInfo start = new ProcessStartInfo();
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Script");
            string pyPath = Path.Combine(folderPath, "dropPiece.py");
            start.FileName = "D:\\anaconda\\python.exe"; // 指定 python 解释器的路径，如果已经在 PATH 环境变量中，则不需要完整路径
            start.Arguments = $"{pyPath} \"{matrixJson}\" \"{listJson}\""; // 指定脚本文件和参数
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true; // 重定向标准错误流
            start.CreateNoWindow = true;
            //同步执行
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    string[] lines = result.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string lastLine = lines.LastOrDefault();
                    string[] resultArray = lastLine.Split(',');
                    return new int[] { int.Parse(resultArray[0]), int.Parse(resultArray[1]) };
                }
            }
        }

    }
}

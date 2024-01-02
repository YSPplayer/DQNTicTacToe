using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;

namespace TicTacToe.DQN
{
	class ScriptManage
	{
        private static string ConvertArrayToJson(int[,] array)
        {
            return JsonConvert.SerializeObject(array);
        }

        private static string ConvertListToJson(List<int[]> list)
        {
            return JsonConvert.SerializeObject(list);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyMusicGameNew
{
    public class Common
    {
        private Common() { }

        public static string ReadFile(string filePath)
        {
            string str = "";
            using (System.IO.StreamReader sr = new System.IO.StreamReader(filePath, Encoding.GetEncoding("Shift_JIS")))
            {
                str = sr.ReadToEnd();
            }

            return str;
        }

        public static string GetFilePathOfDependentEnvironment(string filePath)
        {
            return Environment.CurrentDirectory + filePath;
        }

        public static List<string> GetStringListInJson(string jsonPath)
        {
            string jsonStr = ReadFile(jsonPath);
            return JsonConvert.DeserializeObject<List<string>>(jsonStr);
        }
    }
}

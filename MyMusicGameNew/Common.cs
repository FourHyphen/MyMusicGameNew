using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;
using System.IO;

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

        public static TimeSpan CreateTimeSpan(string format, string time)
        {
            string formatRe = format.Replace(":", @"\:").Replace(".", @"\.");
            return TimeSpan.ParseExact(time, formatRe, null);
        }

        public static System.Windows.Media.Imaging.BitmapSource GetImage(string imagePath)
        {
            Bitmap bitmap = new Bitmap(imagePath);
            return GetImage(bitmap);
        }

        private static System.Windows.Media.Imaging.BitmapSource GetImage(Bitmap bitmap)
        {
            return CreateBitmapSourceImage(bitmap);
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static System.Windows.Media.Imaging.BitmapSource CreateBitmapSourceImage(Bitmap bitmapImage)
        {
            // 参考: http://qiita.com/KaoruHeart/items/dc130d5fc00629c1b6ea
            IntPtr handle = bitmapImage.GetHbitmap();
            try
            {
                return System.Windows.Interop.Imaging.
                    CreateBitmapSourceFromHBitmap(handle,
                                                  IntPtr.Zero,
                                                  System.Windows.Int32Rect.Empty,
                                                  System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(handle);
            }
        }

        /// <summary>
        /// return (basis - subtract).toMillisec
        /// </summary>
        public static double DiffMillisecond(TimeSpan basis, TimeSpan subtract)
        {
            return basis.Subtract(subtract).TotalMilliseconds;
        }

        /// <summary>
        /// Jsonファイルを新規作成する
        /// すでに存在する場合は上書きする
        /// </summary>
        /// <param name="instance">出力するクラスのインスタンス(Jsonシリアライズ前)</param>
        /// <param name="filePath"></param>
        public static void CreateJsonFile(object instance, string filePath)
        {
            string jsonData = JsonConvert.SerializeObject(instance);
            WriteNewFile(jsonData, filePath);
        }

        /// <summary>
        /// ファイルに書き込む
        /// すでにファイルが存在する場合は上書きする
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="filePath"></param>
        private static void WriteNewFile(string contents, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                sw.Write(contents);
            }
        }
    }
}

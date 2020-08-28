using System;

namespace TestMyMusicGameNew
{
    public class Common
    {
        public static string GetEnvironmentDirPath()
        {
            if (System.IO.Directory.Exists(Environment.CurrentDirectory + "/GameData"))
            {
                // テストスイートの場合、2回目以降？はすでに設定済み
                return Environment.CurrentDirectory;
            }

            string master = Environment.CurrentDirectory + "../../../";  // テストの単体実行時
            if (!System.IO.Directory.Exists(master + "/GameData"))
            {
                // テストスイートによる全テスト実行時
                master = Environment.CurrentDirectory + "../../../../TestMyMusicGame";
            }

            return master;
        }
    }
}
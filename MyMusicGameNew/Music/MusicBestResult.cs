using Newtonsoft.Json;

namespace MyMusicGameNew
{
    public class MusicBestResult
    {
        [JsonProperty("BestScore")]
        public int BestScore { get; private set; }

        [JsonProperty("BestResultPerfect")]
        public int BestResultPerfect { get; private set; }

        [JsonProperty("BestResultGood")]
        public int BestResultGood { get; private set; }

        [JsonProperty("BestResultBad")]
        public int BestResultBad { get; private set; }

        public string MusicName { get; set; }

        private MusicBestResult() { }

        public MusicBestResult(string musicName, int score, int perfectNum, int goodNum, int badNum)
        {
            BestScore = score;
            BestResultPerfect = perfectNum;
            BestResultGood = goodNum;
            BestResultBad = badNum;
            MusicName = musicName;
        }

        public static MusicBestResult Create(string musicName)
        {
            string musicResultFilePath = GetMusicResultFilePath(musicName);
            if (System.IO.File.Exists(musicResultFilePath))
            {
                return CreateByHistory(musicName, musicResultFilePath);
            }
            else
            {
                return CreateNoPlayData(musicName);
            }
        }

        private static string GetMusicResultFilePath(string musicName)
        {
            return Common.GetFilePathOfDependentEnvironment("/GameData/MusicResult/" + musicName + ".json");
        }

        private static MusicBestResult CreateByHistory(string musicName, string musicResultFilePath)
        {
            string jsonStr = Common.ReadFile(musicResultFilePath);
            MusicBestResult mbr = JsonConvert.DeserializeObject<MusicBestResult>(jsonStr);
            mbr.MusicName = musicName;

            return mbr;
        }

        private static MusicBestResult CreateNoPlayData(string musicName)
        {
            MusicBestResult mbr = new MusicBestResult();
            mbr.BestScore = 0;
            mbr.BestResultPerfect = 0;
            mbr.BestResultGood = 0;
            mbr.BestResultBad = 0;
            mbr.MusicName = musicName;
            return mbr;
        }

        public void Save()
        {
            string musicResultFilePath = GetMusicResultFilePath(MusicName);
            CreateSaveDir(musicResultFilePath);
            Common.CreateJsonFile(this, musicResultFilePath);
        }

        private void CreateSaveDir(string musicResultFilePath)
        {
            string dirPath = System.IO.Path.GetDirectoryName(musicResultFilePath);
            System.IO.Directory.CreateDirectory(dirPath);
        }

        public void Reset()
        {
            string musicResultFilePath = GetMusicResultFilePath(MusicName);
            Common.DeleteFile(musicResultFilePath);
            BestScore = 0;
            BestResultPerfect = 0;
            BestResultGood = 0;
            BestResultBad = 0;
        }
    }
}

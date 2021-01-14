using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static MusicBestResult Create(string musicName)
        {
            string musicResultFilePath = Common.GetFilePathOfDependentEnvironment("/GameData/MusicResult/" + musicName + ".json");
            if (System.IO.File.Exists(musicResultFilePath))
            {
                return CreateByHistory(musicName, musicResultFilePath);
            }
            else
            {
                return CreateNoPlayData(musicName);
            }
        }

        private static MusicBestResult CreateByHistory(string musicName, string musicResultFilePath)
        {
            string jsonPath = Common.GetFilePathOfDependentEnvironment(musicResultFilePath);
            string jsonStr = Common.ReadFile(jsonPath);
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
    }
}

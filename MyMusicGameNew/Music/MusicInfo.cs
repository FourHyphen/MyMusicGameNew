using Newtonsoft.Json;

namespace MyMusicGameNew
{
    public class MusicInfo
    {
        [JsonProperty("MusicData")]
        public string MusicDataFilePath { get; private set; }

        [JsonProperty("Note")]
        public string NoteDataFilePath { get; private set; }

        [JsonProperty("TimeSecond")]
        public int TimeSecond { get; private set; }
    }
}

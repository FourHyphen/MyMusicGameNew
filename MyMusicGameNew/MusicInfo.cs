using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class MusicInfo
    {
        [JsonProperty("MusicData")]
        public string MusicData { get; private set; }

        [JsonProperty("Note")]
        public string Note { get; private set; }

        [JsonProperty("TimeSecond")]
        public int TimeSecond { get; private set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyMusicGameNew
{
    public class Note
    {
        [JsonProperty("FormatOfJudgeFromMusicStart")]
        private string FormatOfJudgeFromMusicStart { get; set; }

        [JsonProperty("StringOfJudgeFromMusicStart")]
        private string StringOfJudgeFromMusicStart { get; set; }

        public Note() { }

        public TimeSpan JudgeOfJustTiming
        {
            get
            {
                // TODO: TimeSpanがnull非許容なので毎回作成するが、本来は初回要求時のみnewしたい
                return Common.CreateTimeSpan(FormatOfJudgeFromMusicStart, StringOfJudgeFromMusicStart);
            }
        }
    }
}
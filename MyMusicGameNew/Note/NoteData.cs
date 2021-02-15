using System;
using Newtonsoft.Json;

namespace MyMusicGameNew
{
    public class NoteData
    {
        [JsonProperty("FormatOfJudgeFromMusicStart")]
        private string FormatOfJudgeFromMusicStart { get; set; }

        [JsonProperty("StringOfJudgeFromMusicStart")]
        private string StringOfJudgeFromMusicStart { get; set; }

        [JsonProperty("XJudgeLinePosition")]
        public int XJudgeLinePosition { get; private set; }

        public NoteData(int xJudgeLinePosition, TimeSpan judgeOfJustTiming)
        {
            XJudgeLinePosition = xJudgeLinePosition;
            _JudgeOfJustTiming = judgeOfJustTiming;
        }

        private TimeSpan _JudgeOfJustTiming { get; set; }

        public TimeSpan JudgeOfJustTiming
        {
            get
            {
                // TimeSpanは未定義の場合nullではなく"00:00:00"、初期化されたかの確認は初期値との比較で行う
                if (_JudgeOfJustTiming == new TimeSpan())
                {
                    _JudgeOfJustTiming = Common.CreateTimeSpan(FormatOfJudgeFromMusicStart, StringOfJudgeFromMusicStart);
                }
                return _JudgeOfJustTiming;
            }
        }
    }
}

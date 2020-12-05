using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class NoteJudge
    {
        public enum JudgeType
        {
            Perfect,
            Good,
            Bad,
            NotYet
        }

        private NoteData _NoteData { get; set; }

        public JudgeType JudgeResult { get; private set; } = JudgeType.NotYet;

        public NoteJudge(NoteData noteData)
        {
            _NoteData = noteData;
        }

        public void Judge(TimeSpan time)
        {
            double diffMillsec = Math.Abs(Common.DiffMillisecond(_NoteData.JudgeOfJustTiming, time));

            // TODO: 判定タイミングの外部管理化
            if (diffMillsec < 100)  // 1[ms]
            {
                JudgeResult = JudgeType.Perfect;
            }
            else if (diffMillsec < 200)  // 2[ms]
            {
                JudgeResult = JudgeType.Good;
            }
            else if (diffMillsec < 400)  // 4[ms]
            {
                JudgeResult = JudgeType.Bad;
            }
            else
            {
                JudgeResult = JudgeType.NotYet;
            }
        }
    }
}

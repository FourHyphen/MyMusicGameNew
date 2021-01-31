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

        private NoteJudge() { }

        public static JudgeType Judge(NoteData noteData, TimeSpan time)
        {
            double diffMillsec = Math.Abs(Common.DiffMillisecond(noteData.JudgeOfJustTiming, time));

            // TODO: 判定タイミングの外部管理化
            if (diffMillsec < 100)  // 1[ms]
            {
                return JudgeType.Perfect;
            }
            else if (diffMillsec < 200)  // 2[ms]
            {
                return JudgeType.Good;
            }
            else if (diffMillsec < 400)  // 4[ms]
            {
                return JudgeType.Bad;
            }
            else
            {
                return JudgeType.NotYet;
            }
        }

        public static bool DidNotePassJudgeLineForAWhile(NoteData noteData, TimeSpan time)
        {
            double diffMillsec = Common.DiffMillisecond(time, noteData.JudgeOfJustTiming);

            // TODO: 判定タイミングの外部管理化
            return (diffMillsec > 500);  // 5[ms]
        }
    }
}

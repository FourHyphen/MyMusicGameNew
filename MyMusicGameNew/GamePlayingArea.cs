using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class GamePlayingArea
    {
        private int JudgeLineYFromAreaTop { get; set; }

        private int PlayAreaWidth { get; set; }

        private int PlayAreaHeight { get; set; }

        private double NoteSpeedRate { get; }

        public GamePlayingArea(int playAreaWidth, int playAreaHeight, int judgeLineYFromAreaTop, double noteSpeedRate)
        {
            PlayAreaWidth = playAreaWidth;
            PlayAreaHeight = playAreaHeight;
            JudgeLineYFromAreaTop = judgeLineYFromAreaTop;
            NoteSpeedRate = noteSpeedRate;
        }

        public bool IsInsidePlayArea(Note note)
        {
            double x = note.NowX;
            double y = note.NowY;
            return ((0.0 <= x && x < PlayAreaWidth) &&
                    (0.0 <= y && y < PlayAreaHeight));
        }

        public System.Windows.Point CalcNowPoint(NoteData noteData, TimeSpan now, double noteSpeedYPerSec)
        {
            double diffMillsec = Common.DiffMillisecond(noteData.JudgeOfJustTiming, now);
            return new System.Windows.Point(CalcNowX(noteData), CalcNowY(diffMillsec, noteSpeedYPerSec));
        }

        private double CalcNowX(NoteData noteData)
        {
            return GetLinePointX(noteData.XJudgeLinePosition);
        }

        private double CalcNowY(double diffMillisec, double noteSpeedYPerSec)
        {
            double dist = (diffMillisec / 1000.0) * (noteSpeedYPerSec * NoteSpeedRate);
            return JudgeLineYFromAreaTop - dist;
        }

        public double GetLinePointX(int lineNum)
        {
            int basis = (int)((double)PlayAreaWidth * 0.33333);
            return basis * lineNum;
        }

        public double GetLinePointY()
        {
            return JudgeLineYFromAreaTop;
        }

        public int ConvertXLine(double x)
        {
            // TODO: 汚い...
            double halfWidth = PlayAreaWidth / 2;
            if (x <= halfWidth)
            {
                return 1;
            }
            else if (x > halfWidth)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        public int ConvertXLine(Keys.EnableKeys key)
        {
            if (key == Keys.EnableKeys.JudgeLine1)
            {
                return 1;
            }
            else if (key == Keys.EnableKeys.JudgeLine2)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
    }
}

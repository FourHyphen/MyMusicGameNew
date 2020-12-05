using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class GamePlayingArea
    {
        public int JudgeLineYFromAreaTop { get; private set; }

        private int PlayAreaWidth { get; set; }

        private int PlayAreaHeight { get; set; }

        public GamePlayingArea(int playAreaWidth, int playAreaHeight)
        {
            PlayAreaWidth = playAreaWidth;
            PlayAreaHeight = playAreaHeight;
            JudgeLineYFromAreaTop = playAreaHeight - 100;
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
            int basis = (int)((double)PlayAreaWidth * 0.33333);
            return basis * noteData.XJudgeLinePosition;
        }

        private double CalcNowY(double diffMillisec, double noteSpeedYPerSec)
        {
            double dist = (diffMillisec / 1000.0) * noteSpeedYPerSec;
            return JudgeLineYFromAreaTop - dist;
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
    }
}

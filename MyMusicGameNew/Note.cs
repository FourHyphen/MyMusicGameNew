using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class Note
    {
        private double NoteSpeedYPerSec { get; set; }

        public int JudgeLineYFromAreaTop { get; private set; }

        private int PlayAreaWidth { get; set; }

        private int PlayAreaHeight { get; set; }

        private NoteData _NoteData { get; set; }

        public NoteImage Image { get; private set; }

        private NoteJudge _NoteJudge { get; set; }

        public double NowX { get; set; }

        public double NowY { get; set; }

        public int XLine
        {
            get
            {
                return _NoteData.XJudgeLinePosition;
            }
        }

        public NoteJudge.JudgeType JudgeResult
        {
            get
            {
                return _NoteJudge.JudgeResult;
            }
        }

        public Note(NoteData noteData, int playAreaWidth, int playAreaHeight, double noteSpeedYPerSec = 300.0)
        {
            _NoteData = noteData;
            _NoteJudge = new NoteJudge(_NoteData);
            PlayAreaWidth = playAreaWidth;
            PlayAreaHeight = playAreaHeight;
            NoteSpeedYPerSec = noteSpeedYPerSec;
            JudgeLineYFromAreaTop = playAreaHeight - 100;
        }

        public void CalcNowPoint(TimeSpan now)
        {
            double diffMillsec = Common.DiffMillisecond(_NoteData.JudgeOfJustTiming, now);
            NowX = CalcNowX();
            NowY = CalcNowY(diffMillsec);
            if (Image != null)
            {
                Image.SetNowCoordinate(NowX, NowY);
            }
        }

        private double CalcNowX()
        {
            int basis = (int)((double)PlayAreaWidth * 0.33333);
            return basis * _NoteData.XJudgeLinePosition;
        }

        private double CalcNowY(double diffMillisec)
        {
            double dist = (diffMillisec / 1000.0) * NoteSpeedYPerSec;
            return JudgeLineYFromAreaTop - dist;
        }

        public bool IsInsidePlayArea()
        {
            return ((0.0 <= NowX && NowX < PlayAreaWidth) &&
                    (0.0 <= NowY && NowY < PlayAreaHeight));
        }

        public void InitImage(int index)
        {
            Image = new NoteImage(index);
        }

        public void Judge(TimeSpan time)
        {
            _NoteJudge.Judge(time);
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

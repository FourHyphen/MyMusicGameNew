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

        public double NowX { get; set; }

        public double NowY { get; set; }

        public Note(NoteData noteData, int playAreaWidth, int playAreaHeight, double noteSpeedYPerSec = 300.0)
        {
            _NoteData = noteData;
            PlayAreaWidth = playAreaWidth;
            PlayAreaHeight = playAreaHeight;
            NoteSpeedYPerSec = noteSpeedYPerSec;
            JudgeLineYFromAreaTop = playAreaHeight - 100;
        }

        public void CalcNowPoint(TimeSpan now)
        {
            double diffMillsec = DiffMillisecond(_NoteData.JudgeOfJustTiming, now);
            NowX = CalcNowX();
            NowY = CalcNowY(diffMillsec);
            if (Image != null)
            {
                Image.SetNowCoordinate(NowX, NowY);
            }
        }

        private double DiffMillisecond(TimeSpan basis, TimeSpan subtract)
        {
            // return (basis - subtract).toMillisec
            return basis.Subtract(subtract).TotalMilliseconds;
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
    }
}

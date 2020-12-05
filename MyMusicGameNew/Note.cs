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

        private GamePlayingArea _GamePlayingArea { get; set; }

        private NoteData _NoteData { get; set; }

        private NoteImage Image { get; set; }

        private NoteJudge _NoteJudge { get; set; }

        public System.Windows.Point NowPoint { get; set; }

        public double NowX
        {
            get
            {
                return NowPoint.X;
            }
        }

        public double NowY
        {
            get
            {
                return NowPoint.Y;
            }
        }

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

        public System.Windows.Controls.Image DisplayImage
        {
            get
            {
                return Image.DisplayImage;
            }
        }

        public Note(NoteData noteData, GamePlayingArea area, double noteSpeedYPerSec = 300.0)
        {
            _GamePlayingArea = area;
            _NoteData = noteData;
            _NoteJudge = new NoteJudge(_NoteData);
            NowPoint = new System.Windows.Point();
            NoteSpeedYPerSec = noteSpeedYPerSec;
        }

        public void CalcNowPoint(TimeSpan now)
        {
            NowPoint = _GamePlayingArea.CalcNowPoint(_NoteData, now, NoteSpeedYPerSec);
            
            if (Image != null)
            {
                Image.SetNowCoordinate(NowPoint);
            }
        }

        public void InitImage(int index)
        {
            Image = new NoteImage(index);
        }

        public void Judge(TimeSpan time)
        {
            _NoteJudge.Judge(time);
        }

        public bool IsInsidePlayArea()
        {
            return _GamePlayingArea.IsInsidePlayArea(NowPoint);
        }

        public bool AlreadyJudged(double x)
        {
            int line = _GamePlayingArea.ConvertXLine(x);
            if (XLine == line)
            {
                return false;
            }

            return AlreadyJudged();
        }

        public bool AlreadyJudged()
        {
            return (JudgeResult != NoteJudge.JudgeType.NotYet);
        }

        public void SetVisible()
        {
            Image.SetVisible();
        }
    }
}

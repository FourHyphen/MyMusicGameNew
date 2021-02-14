using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class Note
    {
        private double NoteSpeedXPerSec { get; set; }

        private double NoteSpeedYPerSec { get; set; }

        private NoteData _NoteData { get; set; }

        private NoteImage Image { get; set; }

        private System.Windows.Point NowPoint { get; set; }

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

        public NoteJudge.JudgeType JudgeResult { get; private set; } = NoteJudge.JudgeType.NotYet;

        public Note(NoteData noteData, double noteSpeedXPerSec = 300.0, double noteSpeedYPerSec = 300.0)
        {
            _NoteData = noteData;
            NowPoint = new System.Windows.Point();
            NoteSpeedXPerSec = noteSpeedXPerSec;
            NoteSpeedYPerSec = noteSpeedYPerSec;
        }

        public void CalcNowPoint(GamePlayingArea area, TimeSpan now)
        {
            NowPoint = area.CalcNowPoint(_NoteData, now, NoteSpeedXPerSec, NoteSpeedYPerSec);
            
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
            JudgeResult = NoteJudge.Judge(_NoteData, time);
        }

        public void JudgeBadWhenNotePassedJudgeLineForAWhile(TimeSpan time)
        {
            if (NoteJudge.DidNotePassJudgeLineForAWhile(_NoteData, time))
            {
                JudgeResult = NoteJudge.JudgeType.Bad;
            }
        }

        public bool AlreadyJudged()
        {
            return (JudgeResult != NoteJudge.JudgeType.NotYet);
        }

        public void DisplayPlayArea(System.Windows.Controls.UIElementCollection collection)
        {
            Image.DisplayPlayArea(collection);
        }

        public void HidePlayArea(System.Windows.Controls.UIElementCollection collection)
        {
            Image.HidePlayArea(collection);
        }
    }
}

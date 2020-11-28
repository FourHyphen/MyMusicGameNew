using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMusicGameNew;

namespace TestMyMusicGameNew
{
    public class EmurateNote
    {
        private static readonly int PlayAreaX = 800;
        private static readonly int PlayAreaY = 600;
        private static readonly double NoteSpeedYPerSec = 300.0;

        public int XLine { get; private set; }

        public TimeSpan JustTiming { get; private set; }

        public TimeSpan BadTooFastTiming
        {
            get
            {
                return JustTiming.Subtract(new TimeSpan(0, 0, 0, 0, 350));    // 400[ms]早ければBad判定、少し余裕をもたせて350[ms]
            }
        }

        private static readonly double NotInsidePlayAreaWhenMusicStartJustTiming = PlayAreaY * 2;

        public Note Note { get; private set; }

        public double NowX { get { return Note.NowX; } }

        public double NowY { get { return Note.NowY; } }

        public EmurateNote(int noteNumber)
        {
            Init(noteNumber);
        }

        private void Init(int noteNumber)
        {
            if (noteNumber == 1)
            {
                XLine = 1;
                JustTiming = new TimeSpan(0, 0, 0, 1, 0);  // 1[s]
            }
            else if (noteNumber == 2)
            {
                XLine = 2;
                JustTiming = new TimeSpan(0, 0, 0, 2, 500);  // 2.5[s]
            }
            NoteData nd = new NoteData(XLine, JustTiming);
            Note = new Note(nd, PlayAreaX, PlayAreaY, NoteSpeedYPerSec);
        }

        public void SetNowPoint(TimeSpan time)
        {
            Note.CalcNowPoint(time);
        }

        public System.Windows.Point EmurateCalcPoint(TimeSpan elapsedTimeFromGameStart)
        {
            double note1XPoint = XLine * (int)((double)PlayAreaX * 0.33333);
            double diff = JustTiming.Subtract(elapsedTimeFromGameStart).TotalMilliseconds;
            double note1YPoint = Note.JudgeLineYFromAreaTop - (diff * NoteSpeedYPerSec / 1000);
            return new System.Windows.Point(note1XPoint, note1YPoint);
        }

        public System.Windows.Point EmurateCalcJustJudgeLinePoint()
        {
            double note1XPoint = XLine * (int)((double)PlayAreaX * 0.33333);
            double note1YPoint = Note.JudgeLineYFromAreaTop;
            return new System.Windows.Point(note1XPoint, note1YPoint);
        }

        public bool IsInsidePlayArea()
        {
            return Note.IsInsidePlayArea();
        }

        public static Note GetNoteNotInsidePlayAreaWhenMusicStartJustTiming(int noteNumber)
        {
            EmurateNote en = new EmurateNote(noteNumber);
            NoteData nd = new NoteData(en.XLine, en.JustTiming);
            return new Note(nd, PlayAreaX, PlayAreaY, NotInsidePlayAreaWhenMusicStartJustTiming);
        }
    }
}

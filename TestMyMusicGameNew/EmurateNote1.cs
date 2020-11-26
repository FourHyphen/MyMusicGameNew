using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMusicGameNew;

namespace TestMyMusicGameNew
{
    public class EmurateNote1
    {
        private static readonly int PlayAreaX = 800;
        private static readonly int PlayAreaY = 600;
        private static readonly double NoteSpeedYPerSec = 300.0;
        private static readonly int XLine = 1;
        public static TimeSpan JustTiming { get; private set; } = new TimeSpan(0, 0, 0, 1, 0);  // 1[s]

        private static readonly double NotInsidePlayAreaWhenMusicStartJustTiming = PlayAreaY * 2;

        public Note Note { get; private set; }

        public double NowX { get { return Note.NowX; } }

        public double NowY { get { return Note.NowY; } }

        public EmurateNote1()
        {
            Init();
        }

        private void Init()
        {
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

        public System.Windows.Point EmurateCalcJustTimingPoint()
        {
            double note1XPoint = XLine * (int)((double)PlayAreaX * 0.33333);
            double note1YPoint = Note.JudgeLineYFromAreaTop;
            return new System.Windows.Point(note1XPoint, note1YPoint);
        }

        public bool IsInsidePlayArea()
        {
            return Note.IsInsidePlayArea();
        }

        public static Note GetNoteNotInsidePlayAreaWhenMusicStartJustTiming()
        {
            NoteData nd = new NoteData(XLine, JustTiming);
            return new Note(nd, PlayAreaX, PlayAreaY, NotInsidePlayAreaWhenMusicStartJustTiming);
        }
    }
}

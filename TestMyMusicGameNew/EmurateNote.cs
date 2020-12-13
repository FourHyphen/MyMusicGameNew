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
        private static readonly double NoteSpeedYPerSec = 300.0;

        public int XLine { get; private set; }

        public TimeSpan JustTiming { get; private set; }

        public TimeSpan BadTooFastTiming
        {
            get
            {
                return JustTiming.Subtract(new TimeSpan(0, 0, 0, 0, 350));    // 200 ～ 400[ms]早ければBad判定
            }
        }

        public TimeSpan BadTooSlowTiming
        {
            get
            {
                return JustTiming.Add(new TimeSpan(0, 0, 0, 0, 350));    // 200 ～ 400[ms]遅ければBad判定、少し余裕をもたせて350[ms]
            }
        }

        public TimeSpan PassedBadTiming
        {
            get
            {
                return JustTiming.Add(new TimeSpan(0, 0, 0, 0, 550));    // 500[ms]遅ければ見逃しのBad判定、少し余裕を持たせて550[ms]
            }
        }

        private int PlayAreaX { get; set; }

        private int PlayAreaY { get; set; }

        private double JudgeLineYFromAreaTop { get { return PlayAreaY - 100.0; } }

        private double NotInsidePlayAreaWhenMusicStartJustTiming { get { return PlayAreaY * 2; } }

        public Note Note { get; private set; }

        public double NowX { get { return Note.NowX; } }

        public double NowY { get { return Note.NowY; } }

        private GamePlayingArea _GamePlayingArea { get; set; }

        public EmurateNote(int playAreaWidth, int playAreaHeight, int noteNumber)
        {
            Init(noteNumber);
            _GamePlayingArea = new GamePlayingArea(playAreaWidth, playAreaHeight);
            PlayAreaX = playAreaWidth;
            PlayAreaY = playAreaHeight;
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
            Note = new Note(nd, NoteSpeedYPerSec);
        }

        public void SetNowPoint(TimeSpan time)
        {
            Note.CalcNowPoint(_GamePlayingArea, time);
        }

        public System.Windows.Point EmurateCalcPoint(TimeSpan elapsedTimeFromGameStart)
        {
            double note1XPoint = XLine * (int)((double)PlayAreaX * 0.33333);
            double diff = JustTiming.Subtract(elapsedTimeFromGameStart).TotalMilliseconds;
            double note1YPoint = JudgeLineYFromAreaTop - (diff * NoteSpeedYPerSec / 1000);
            return new System.Windows.Point(note1XPoint, note1YPoint);
        }

        public System.Windows.Point EmurateCalcJustJudgeLinePoint()
        {
            double note1XPoint = XLine * (int)((double)PlayAreaX * 0.33333);
            double note1YPoint = JudgeLineYFromAreaTop;
            return new System.Windows.Point(note1XPoint, note1YPoint);
        }

        public bool IsInsidePlayArea()
        {
            return _GamePlayingArea.IsInsidePlayArea(Note);
        }
    }
}

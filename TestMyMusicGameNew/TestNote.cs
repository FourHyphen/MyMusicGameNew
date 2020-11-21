using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMusicGameNew;

namespace TestMyMusicGameNew
{
    [TestClass]
    public class TestNote
    {
        private static readonly int PlayAreaX = 800;
        private static readonly int PlayAreaY = 600;
        private static readonly double NoteSpeedYPerSec = 300.0;
        private static readonly int Note1XLine = 1;
        private static readonly TimeSpan Note1Time = new TimeSpan(0, 0, 0, 1, 0);  // 1[s]
        private static readonly TimeSpan ZeroTime = new TimeSpan(0, 0, 0, 0, 0);

        private static readonly double NotInsidePlayAreaWhenMusicStartJustTiming = PlayAreaY * 2;

        [TestMethod]
        public void TestCorrectNoteCoordinateAtTheTime()
        {
            NoteData nd = new NoteData(Note1XLine, Note1Time);
            Note note = new Note(nd, PlayAreaX, PlayAreaY, NoteSpeedYPerSec);

            // 初期座標の確認
            note.CalcNowPoint(ZeroTime);

            double note1XPoint = Note1XLine * (int)((double)PlayAreaX * 0.33333);
            double note1YPoint = note.JudgeLineYFromAreaTop - (Note1Time.TotalSeconds * NoteSpeedYPerSec);
            Assert.AreEqual(expected: note1XPoint, actual: note.NowX);
            Assert.AreEqual(expected: note1YPoint, actual: note.NowY);

            // 判定線とちょうど重なるときの確認
            note.CalcNowPoint(Note1Time);

            double note1XPointJustTiming = note1XPoint;
            double note1YPointJustTiming = note.JudgeLineYFromAreaTop;
            Assert.AreEqual(expected: note1XPointJustTiming, actual: note.NowX);
            Assert.AreEqual(expected: note1YPointJustTiming, actual: note.NowY);
        }

        [TestMethod]
        public void TestCorrectWhetherInsidePlayArea()
        {
            NoteData nd = new NoteData(Note1XLine, Note1Time);

            Note noteOutside = new Note(nd, PlayAreaX, PlayAreaY, NotInsidePlayAreaWhenMusicStartJustTiming);
            noteOutside.CalcNowPoint(ZeroTime);
            Assert.IsFalse(noteOutside.IsInsidePlayArea());

            Note noteInside = new Note(nd, PlayAreaX, PlayAreaY, NoteSpeedYPerSec);
            noteInside.CalcNowPoint(ZeroTime);
            Assert.IsTrue(noteInside.IsInsidePlayArea());
        }
    }
}

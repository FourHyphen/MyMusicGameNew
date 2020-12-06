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
        private static readonly TimeSpan ZeroTime = new TimeSpan(0, 0, 0, 0, 0);
        private static readonly int PlayAreaX = 800;
        private static readonly int PlayAreaY = 600;

        [TestMethod]
        public void TestCorrectNoteCoordinateAtTheTime()
        {
            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);

            // 初期座標の確認
            emurateNote1.SetNowPoint(ZeroTime);
            System.Windows.Point initPoint = emurateNote1.EmurateCalcPoint(ZeroTime);
            Assert.AreEqual(expected: initPoint.X, actual: emurateNote1.NowX);
            Assert.AreEqual(expected: initPoint.Y, actual: emurateNote1.NowY);

            // 判定線とちょうど重なるときの確認
            emurateNote1.SetNowPoint(emurateNote1.JustTiming);
            System.Windows.Point justTimingPoint = emurateNote1.EmurateCalcJustJudgeLinePoint();

            Assert.AreEqual(expected: justTimingPoint.X, actual: emurateNote1.NowX);
            Assert.AreEqual(expected: justTimingPoint.Y, actual: emurateNote1.NowY);
        }

        [TestMethod]
        public void TestCorrectWhetherInsidePlayArea()
        {
            EmurateNote en = new EmurateNote(PlayAreaX, PlayAreaY, 1);
            NoteData nd = new NoteData(en.XLine, en.JustTiming);
            Note noteOutside = new Note(nd, PlayAreaY * 10);    // 10倍ならNoteSpeedYPerSecによらず、確実に初期値が画面外になる

            GamePlayingArea area = new GamePlayingArea(PlayAreaX, PlayAreaY);
            noteOutside.CalcNowPoint(area, ZeroTime);
            Assert.IsFalse(area.IsInsidePlayArea(noteOutside));

            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);
            emurateNote1.SetNowPoint(ZeroTime);
            Assert.IsTrue(emurateNote1.IsInsidePlayArea());
        }

        [TestMethod]
        public void TestJudgeBadWhenNotePassedJudgeLineForAWhile()
        {
            TimeSpan justTiming = new TimeSpan(0, 0, 0, 1, 0);
            NoteData noteData = new NoteData(1, justTiming);
            Note note = new Note(noteData);

            note.JudgeBadWhenNotePassedJudgeLineForAWhile(justTiming);
            Assert.AreEqual(expected: NoteJudge.JudgeType.NotYet, actual: note.JudgeResult);

            TimeSpan passedBadTiming = new TimeSpan(0, 0, 0, 1, 501);    // ちょうどが500[ms] + 余裕で1[ms]
            note.JudgeBadWhenNotePassedJudgeLineForAWhile(passedBadTiming);
            Assert.AreEqual(expected: NoteJudge.JudgeType.Bad, actual: note.JudgeResult);
        }
    }
}

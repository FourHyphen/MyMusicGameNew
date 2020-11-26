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

        [TestMethod]
        public void TestCorrectNoteCoordinateAtTheTime()
        {
            EmurateNote1 emurateNote1 = new EmurateNote1();

            // 初期座標の確認
            emurateNote1.SetNowPoint(ZeroTime);
            System.Windows.Point initPoint = emurateNote1.EmurateCalcPoint(ZeroTime);
            Assert.AreEqual(expected: initPoint.X, actual: emurateNote1.NowX);
            Assert.AreEqual(expected: initPoint.Y, actual: emurateNote1.NowY);

            // 判定線とちょうど重なるときの確認
            emurateNote1.SetNowPoint(EmurateNote1.JustTiming);
            System.Windows.Point justTimingPoint = emurateNote1.EmurateCalcJustTimingPoint();

            Assert.AreEqual(expected: justTimingPoint.X, actual: emurateNote1.NowX);
            Assert.AreEqual(expected: justTimingPoint.Y, actual: emurateNote1.NowY);
        }

        [TestMethod]
        public void TestCorrectWhetherInsidePlayArea()
        {
            Note noteOutside = EmurateNote1.GetNoteNotInsidePlayAreaWhenMusicStartJustTiming();
            noteOutside.CalcNowPoint(ZeroTime);
            Assert.IsFalse(noteOutside.IsInsidePlayArea());

            EmurateNote1 emurateNote1 = new EmurateNote1();
            emurateNote1.SetNowPoint(ZeroTime);
            Assert.IsTrue(emurateNote1.IsInsidePlayArea());
        }
    }
}

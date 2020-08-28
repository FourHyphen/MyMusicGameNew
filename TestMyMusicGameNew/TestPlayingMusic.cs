using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMusicGameNew;

namespace TestMyMusicGameNew
{
    [TestClass]
    public class TestPlayingMusic
    {
        [TestMethod]
        public void TestCreate()
        {
            PlayingMusic fake = PlayingMusicFactory.Create("test", isTest: true);
            Assert.IsTrue(fake.GetType().ToString().Contains("Fake"));

            PlayingMusic play = PlayingMusicFactory.Create("test", isTest: false);
            Assert.IsFalse(play.GetType().ToString().Contains("Fake"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMusicGameNew;

namespace TestMyMusicGameNew
{
    [TestClass]
    public class TestMusic
    {
        private const int Test1MusicTimeSecond = 4;
        private const int Test1MusicNoteNum = 2;

        private string BeforeEnvironment { get; set; }

        [TestInitialize]
        public void TestInit()
        {
            BeforeEnvironment = Environment.CurrentDirectory;
            Environment.CurrentDirectory = Common.GetEnvironmentDirPath();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Environment.CurrentDirectory = BeforeEnvironment;
        }

        [TestMethod]
        public void TestMusicFactory()
        {
            string name = "test1";
            Music music = new MusicFactory().Create(name, true);

            Assert.AreEqual(expected: name, actual: music.Name);
            Assert.AreEqual(expected: Test1MusicTimeSecond * 1000, actual: music.TimeMilliSecond);
            Assert.AreEqual(expected: Test1MusicNoteNum, actual: music.Notes.Count);
        }
    }
}

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
            Music music = new MusicFactory().Create(name, isTest: true);

            Assert.AreEqual(expected: name, actual: music.Name);
            Assert.AreEqual(expected: 4, actual: music.TimeSecond);
            Assert.AreEqual(expected: 2, actual: music.Notes.Count);
            music.PlayAsync();  // 例外なければOK
        }
    }
}

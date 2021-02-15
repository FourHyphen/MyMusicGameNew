using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMusicGameNew;

namespace TestMyMusicGameNew
{
    [TestClass]
    public class TestMusicBestResult
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
        public void TestNotCrashIfSaveDirIsNotFound()
        {
            string saveDirPath = MyMusicGameNew.Common.GetFilePathOfDependentEnvironment("/GameData/MusicResult");
            if (System.IO.Directory.Exists(saveDirPath))
            {
                System.IO.Directory.Delete(saveDirPath, recursive:true);
            }
            MusicBestResult mbr = new MusicBestResult("test", 0, 0, 0, 0);
            mbr.Save();
        }
    }
}

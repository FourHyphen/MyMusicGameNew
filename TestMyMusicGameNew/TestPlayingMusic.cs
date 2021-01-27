using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMusicGameNew;

namespace TestMyMusicGameNew
{
    [TestClass]
    public class TestPlayingMusic
    {
        [TestMethod]
        public void TestFileNotFoundExceptionIfMusicDataIsNotFound()
        {
            try
            {
                PlayingMusicWav pmw = new PlayingMusicWav("not_found", null);
                Assert.Fail("FileNotFoundException 上がらず");
            }
            catch (FileNotFoundException ex)
            {
                // OK
            }
        }
    }
}

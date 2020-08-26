using System;
using System.Dynamic;
using System.Diagnostics;
using Codeer.Friendly.Dynamic;
using Codeer.Friendly.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMyMusicGameNew
{
    [TestClass]
    public partial class TestFeature
    {
        // 必要なパッケージ
        //  -> Codeer.Friendly
        //  -> Codeer.Friendly.Windows         -> WindowsAppFriend()
        //  -> Codeer.Friendly.Windows.Grasp   -> WindowControl()
        //  -> RM.Friendly.WPFStandardControls -> 各種WPFコントロールを取得するために必要
        // 必要な作業
        //  -> MyMusicGameNewプロジェクトを参照に追加

        private string AttachExeName = "MyMusicGameNew.exe";
        private WindowsAppFriend TestApp;
        private Process TestProcess;
        private dynamic MainWindow;
        private MainWindowDriver Driver;

        private readonly int TestSelectMusicList = 0;

        [TestInitialize]
        public void Init()
        {
            // MainWindowプロセスにattach
            string exePath = System.IO.Path.GetFullPath(AttachExeName);
            TestApp = new WindowsAppFriend(Process.Start(exePath));
            TestProcess = Process.GetProcessById(TestApp.ProcessId);
            MainWindow = TestApp.Type("System.Windows.Application").Current.MainWindow;
            Driver = new MainWindowDriver(MainWindow);
        }

        [TestCleanup]
        public void Cleanup()
        {
            TestApp.Dispose();
            TestProcess.CloseMainWindow();
        }

        [TestMethod]
        public void TestDisplayMusicList()
        {
            Assert.IsTrue(Driver.MusicList.Count() >= 1);
        }

        [TestMethod]
        public void TestDisplaySelectMusic()
        {
            Assert.IsTrue(Driver.GameStatus.Content().Contains("Select"));
            Assert.IsTrue(Driver.GameStatus.Content().Contains("Music"));
        }

        [TestMethod]
        public void TestStartOfGameWhenSelectedMusicAndCallStartOfGame()
        {
            Driver.MusicList.ChangeSelectedIndex(TestSelectMusicList);
            Driver.GameStartButton.Click();
            Assert.IsTrue(Driver.GameStatus.Content().Contains("Playing"));
        }

        [TestMethod]
        public void TestNotStartOfGameWhenNotSelectMusicAndCallStartOfGame()
        {
            Driver.GameStartButton.Click();
            Assert.IsFalse(Driver.GameStatus.Content().Contains("Playing"));
        }

        [TestMethod]
        public void TestPlayMusicWhenStartOfGame()
        {
            Assert.IsTrue(Driver.PlayingMusicStatus.Content().Contains("Not"));
            Driver.MusicList.ChangeSelectedIndex(TestSelectMusicList);
            Driver.GameStartButton.Click();
            Assert.IsFalse(Driver.PlayingMusicStatus.Content().Contains("Not"));
            Assert.IsTrue(Driver.PlayingMusicStatus.Content().Contains("Playing"));
        }
    }
}

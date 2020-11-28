using System;
using System.Dynamic;
using System.Diagnostics;
using System.Threading.Tasks;
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

        private readonly int ExpectedMusicListNum = 2;
        private readonly int ExpectedTest1NotesNum = 2;
        private readonly int Test1MusicIndex = 0;
        private readonly int Test2MusicIndex = 1;
        private readonly int Test2MusicTimeSecond = 2;
        private readonly int NoDisplayNote = 0;
        private readonly int DisplayNote1AndMore = 1;

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
            Assert.AreEqual(expected: ExpectedMusicListNum, actual: Driver.MusicList.Count());
        }

        [TestMethod]
        public void TestDisplaySelectMusic()
        {
            Assert.IsTrue(Driver.GameStatus.Contains("Select"));
            Assert.IsTrue(Driver.GameStatus.Contains("Music"));
        }

        [TestMethod]
        public void TestStartOfGameWhenSelectedMusicAndCallStartOfGame()
        {
            Driver.MusicList.ChangeSelectedIndex(Test1MusicIndex);
            Driver.GameStartButton.Click();
            Assert.IsTrue(Driver.GameStatus.Contains("Playing"));
        }

        [TestMethod]
        public void TestNotStartOfGameWhenNotSelectMusicAndCallStartOfGame()
        {
            Driver.GameStartButton.Click();
            Assert.IsFalse(Driver.GameStatus.Contains("Playing"));
        }

        [TestMethod]
        public void TestPlayMusicWhenStartOfGame()
        {
            Assert.IsTrue(Driver.PlayingMusicStatus.Content().Contains("Not"));
            Driver.MusicList.ChangeSelectedIndex(Test1MusicIndex);
            Driver.GameStartButton.Click();
            Assert.IsFalse(Driver.PlayingMusicStatus.Contains("Not"));
            Assert.IsTrue(Driver.PlayingMusicStatus.Contains("Playing"));
        }

        [TestMethod]
        public void TestGetNotesWhenStartOfGame()
        {
            Driver.MusicList.ChangeSelectedIndex(Test1MusicIndex);
            Driver.GameStartButton.Click();
            Assert.AreEqual(expected: ExpectedTest1NotesNum, actual: int.Parse(Driver.NotesNum.Content()));
        }

        [TestMethod]
        public void TestGameFinishWhenEndMusic()
        {
            Driver.MusicList.ChangeSelectedIndex(Test2MusicIndex);
            Driver.GameStartButton.Click();
            Assert.IsTrue(Driver.PlayingMusicStatus.Contains("Playing"));
            Sleep(Test2MusicTimeSecond + 1);  // 1[s]余裕を持たせる
            Assert.IsTrue(Driver.PlayingMusicStatus.Contains("Finish"));
        }

        [TestMethod]
        public void TestNoteDisplayAndNearbyJudgeLineAfterGameStart()
        {
            Driver.MusicList.ChangeSelectedIndex(Test1MusicIndex);
            Assert.AreEqual(expected: NoDisplayNote, actual: Driver.GetDisplayNotesNum());

            Driver.GameStartButton.Click();
            Assert.IsTrue(Driver.ExistJudgeLine());
            Assert.AreEqual(expected: DisplayNote1AndMore, actual: Driver.GetDisplayNotesNum());

            System.Windows.Point start = Driver.GetDisplayNotesNearestJudgeLine();
            Sleep(0.5);  // sleepしすぎるとノートが画面外に出てしまうので少しの時間にする
            System.Windows.Point moving = Driver.GetDisplayNotesNearestJudgeLine();
            Assert.IsTrue(start.X == moving.X);
            Assert.IsTrue(start.Y < moving.Y);  // 画面下方向を正とする
        }

        [TestMethod]
        public void TestJudgeNoteNearbyJudgeLineWhenLeftClicked()
        {
            // 左クリック入力時のPerfect判定を拾う
            EmurateNote1 emurateNote1 = new EmurateNote1();
            System.Windows.Point justTimingPoint = emurateNote1.EmurateCalcJustTimingPoint();

            Assert.AreEqual(expected: 0, actual: Driver.ResultPerfect.Number());
            Driver.MusicList.ChangeSelectedIndex(Test1MusicIndex);
            Driver.GameStartButton.Click();
            Sleep(EmurateNote1.JustTiming.TotalSeconds);

            Driver.EmurateLeftClickGamePlaying(justTimingPoint);
            Assert.AreEqual(expected: 1, actual: Driver.ResultPerfect.Number());
        }

        private void Sleep(double second)
        {
            Task task = Task.Run(() =>
            {
                System.Threading.Thread.Sleep((int)(second * 1000.0));
            });

            Task.WaitAll(task);
        }
    }
}

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

        private static readonly int PlayAreaX = 800;
        private static readonly int PlayAreaY = 600;

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
        public void TestNotStartOfGameWhenNotSelectMusicAndCallStartOfGame()
        {
            Driver.GameStartButton.Click();
            Assert.IsFalse(Driver.GameStatus.Contains("Playing"));
        }

        [TestMethod]
        public void TestGetNotesWhenStartOfGame()
        {
            Driver.MusicList.ChangeSelectedIndex(Test1MusicIndex);
            Driver.GameStartButton.Click();
            Assert.AreEqual(expected: ExpectedTest1NotesNum, actual: int.Parse(Driver.NotesNum.Content()));
        }

        [TestMethod]
        public void TestPlayingMusicWhenGameStartAndFinish()
        {
            Assert.IsTrue(Driver.PlayingMusicStatus.Content().Contains("Not"));

            Driver.MusicList.ChangeSelectedIndex(Test2MusicIndex);
            Driver.GameStartButton.Click();
            Assert.IsFalse(Driver.PlayingMusicStatus.Contains("Not"));
            Assert.IsTrue(Driver.PlayingMusicStatus.Contains("Playing"));

            Sleep(Test2MusicTimeSecond + 1);  // 1[s]余裕を持たせる
            Assert.IsTrue(Driver.PlayingMusicStatus.Contains("Finish"));
        }

        [TestMethod]
        public void TestJudgeNoteNearbyJudgeLineWhenLeftClicked()
        {
            // 左クリック入力時のPerfect判定のテスト
            // Bad判定やGood判定は画面エミュレートのテストだと環境次第で結果が変わってしまうため、単体テストでカバーすることにした
            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);
            System.Windows.Point clickPointNote1 = emurateNote1.EmurateCalcJustJudgeLinePoint();

            Assert.AreEqual(expected: 0, actual: Driver.ResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.ResultBad.Number());
            Driver.MusicList.ChangeSelectedIndex(Test1MusicIndex);
            Driver.GameStartButton.Click();

            Sleep(emurateNote1.JustTiming.TotalSeconds);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Assert.AreEqual(expected: 1, actual: Driver.ResultPerfect.Number());

            // 結果が判定されたノートは、結果判定直後に画面から消す(非表示にする)
            Assert.AreEqual(expected: 1, actual: Driver.GetDisplayNotesNum(1));
        }

        [TestMethod]
        public void TestNotJudgeNoteIfTooFarJudgeLine()
        {
            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);
            System.Windows.Point clickPointNote1 = emurateNote1.EmurateCalcJustJudgeLinePoint();

            Driver.MusicList.ChangeSelectedIndex(Test1MusicIndex);
            Driver.GameStartButton.Click();

            // 曲開始直後だとゲームが始まってないかもしれないため少しだけwait
            Sleep(0.1);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Assert.AreEqual(expected: 0, actual: Driver.ResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.ResultBad.Number());
        }

        [TestMethod]
        public void TestOnlyOneJudgeNoteIfMultiClicked()
        {
            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);
            System.Windows.Point clickPointNote1 = emurateNote1.EmurateCalcJustJudgeLinePoint();

            Driver.MusicList.ChangeSelectedIndex(Test1MusicIndex);
            Driver.GameStartButton.Click();

            Sleep(emurateNote1.JustTiming.TotalSeconds);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Assert.AreEqual(expected: 1, actual: Driver.ResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.ResultBad.Number());

            Sleep(emurateNote1.BadTooSlowTiming.TotalSeconds - emurateNote1.JustTiming.TotalSeconds);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Assert.AreEqual(expected: 1, actual: Driver.ResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.ResultBad.Number());
        }

        [TestMethod]
        public void TestJudgeBadWhenNotePassedJudgeLineForAWhile()
        {
            // 判定ラインを通り過ぎて一定時間が経っても結果を判定されていないノートはBad判定する
            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);

            Driver.MusicList.ChangeSelectedIndex(Test1MusicIndex);
            Driver.GameStartButton.Click();

            Sleep(emurateNote1.BadTooSlowTiming.TotalSeconds);
            Assert.AreEqual(expected: 0, actual: Driver.ResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.ResultBad.Number());

            Sleep(emurateNote1.PassedBadTiming.TotalSeconds - emurateNote1.BadTooSlowTiming.TotalSeconds);
            Assert.AreEqual(expected: 0, actual: Driver.ResultPerfect.Number());
            Assert.AreEqual(expected: 1, actual: Driver.ResultBad.Number());
        }

        [TestMethod]
        public void TestJudgeNoteNearbyJudgeLineWhenPressKeyboard()
        {
            // キーボード入力による判定
            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);

            Driver.MusicList.ChangeSelectedIndex(Test1MusicIndex);
            Driver.GameStartButton.Click();

            Sleep(emurateNote1.JustTiming.TotalSeconds);
            Driver.EmuratePressKeyboardGamePlaying(MyMusicGameNew.Keys.EnableKeys.JudgeLine1);
            Assert.AreEqual(expected: 1, actual: Driver.ResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.ResultBad.Number());
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

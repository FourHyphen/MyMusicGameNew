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
        private readonly int Test1MusicTimeSecond = 4;
        private readonly int Test2MusicTimeSecond = 2;

        private readonly TimeSpan UntilGameStart = new TimeSpan(0, 0, 3);
        private readonly TimeSpan UntilGameStartFromSuspend = new TimeSpan(0, 0, 3);

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
            GameStart();
            Assert.IsFalse(Driver.GameStatus.Contains("Playing"));
        }

        [TestMethod]
        public void TestGetNotesWhenStartOfGame()
        {
            GameStart(Test1MusicIndex);
            Assert.AreEqual(expected: ExpectedTest1NotesNum, actual: int.Parse(Driver.NotesNum.Content()));
        }

        [TestMethod]
        public void TestPlayingMusicWhenGameStartAndFinish()
        {
            Assert.IsFalse(Driver.PlayingMusicStatus.Content().Contains("Playing"));

            GameStart(Test2MusicIndex);
            Sleep(0.5);    // sleepなしだと失敗したことがある
            Assert.IsTrue(Driver.PlayingMusicStatus.Contains("Playing"));

            Sleep(Test2MusicTimeSecond + 2);  // いくらか余裕を持たせる、1[s]だと失敗したことがある
            Assert.IsTrue(Driver.PlayingMusicStatus.Contains("Finish"));
        }

        [TestMethod]
        public void TestJudgeNoteNearbyJudgeLineWhenLeftClicked()
        {
            // 左クリック入力時のPerfect判定のテスト
            // Bad判定やGood判定は画面エミュレートのテストだと環境次第で結果が変わってしまうため、単体テストでカバーすることにした
            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);
            System.Windows.Point clickPointNote1 = emurateNote1.EmurateCalcJustJudgeLinePoint();

            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultBad.Number());
            GameStart(Test1MusicIndex);

            Sleep(emurateNote1.JustTiming.TotalSeconds);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Assert.AreEqual(expected: 1, actual: Driver.PlayingResultPerfect.Number());

            // 結果が判定されたノートは、結果判定直後に画面から消す(非表示にする)
            Assert.AreEqual(expected: 1, actual: Driver.GetDisplayNotesNum(1));
        }

        [TestMethod]
        public void TestNotJudgeNoteIfTooFarJudgeLine()
        {
            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);
            System.Windows.Point clickPointNote1 = emurateNote1.EmurateCalcJustJudgeLinePoint();

            GameStart(Test1MusicIndex);

            // 曲開始直後だとゲームが始まってないかもしれないため少しだけwait
            Sleep(0.1);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultBad.Number());
        }

        [TestMethod]
        public void TestOnlyOneJudgeNoteIfMultiClicked()
        {
            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);
            System.Windows.Point clickPointNote1 = emurateNote1.EmurateCalcJustJudgeLinePoint();

            GameStart(Test1MusicIndex);

            Sleep(emurateNote1.JustTiming.TotalSeconds);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Assert.AreEqual(expected: 1, actual: Driver.PlayingResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultBad.Number());

            Sleep(emurateNote1.BadTooSlowTiming.TotalSeconds - emurateNote1.JustTiming.TotalSeconds);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Assert.AreEqual(expected: 1, actual: Driver.PlayingResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultBad.Number());
        }

        [TestMethod]
        public void TestJudgeBadWhenNotePassedJudgeLineForAWhile()
        {
            // 判定ラインを通り過ぎて一定時間が経っても結果を判定されていないノートはBad判定する
            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);

            GameStart(Test1MusicIndex);

            Sleep(emurateNote1.BadTooSlowTiming.TotalSeconds);
            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultBad.Number());

            Sleep(emurateNote1.PassedBadTiming.TotalSeconds - emurateNote1.BadTooSlowTiming.TotalSeconds);
            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultPerfect.Number());
            Assert.AreEqual(expected: 1, actual: Driver.PlayingResultBad.Number());
        }

        [TestMethod]
        public void TestJudgeNoteNearbyJudgeLineWhenPressKeyboard()
        {
            // キーボード入力による判定
            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);

            GameStart(Test1MusicIndex);

            Sleep(emurateNote1.JustTiming.TotalSeconds);
            Driver.EmuratePressKeyboardGamePlaying(MyMusicGameNew.Keys.EnableKeys.JudgeLine1);
            Assert.AreEqual(expected: 1, actual: Driver.PlayingResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultBad.Number());
        }

        [TestMethod]
        public void TestGameResultDisplayWhenGameFinished()
        {
            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);
            System.Windows.Point clickPointNote1 = emurateNote1.EmurateCalcJustJudgeLinePoint();

            GameStart(Test1MusicIndex);

            // 1つPerfectで拾い、1つ見逃しBadとして、ゲーム結果画面の表示をテストする
            Sleep(emurateNote1.JustTiming.TotalSeconds);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);

            Sleep(Test1MusicTimeSecond - emurateNote1.JustTiming.TotalSeconds + 1);    // 1[s]余裕を持たせる
            Assert.AreEqual(expected: 1, actual: Driver.FinishResultPerfect.Number());
            Assert.AreEqual(expected: 1, actual: Driver.FinishResultBad.Number());
        }

        [TestMethod]
        public void TestResetPlayingResultWhenNextGameStart()
        {
            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);
            System.Windows.Point clickPointNote1 = emurateNote1.EmurateCalcJustJudgeLinePoint();

            GameStart(Test1MusicIndex);
            Sleep(emurateNote1.JustTiming.TotalSeconds);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);

            Sleep(Test1MusicTimeSecond - emurateNote1.JustTiming.TotalSeconds + 1);    // 1[s]余裕を持たせる
            Driver.EmurateGamePlayingResultOKButtonClick();

            GameStart(Test1MusicIndex);
            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultBad.Number());
        }

        [TestMethod]
        public void TestSavePlayResultWhenGameFinish()
        {
            // 1曲プレイ -> 曲選択画面でその結果がベストとして表示されればOK
            Driver.ResetBestScore(Test1MusicIndex);

            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);
            System.Windows.Point clickPointNote1 = emurateNote1.EmurateCalcJustJudgeLinePoint();

            Driver.MusicList.ChangeSelectedIndex(Test1MusicIndex);
            Assert.AreEqual(expected: 0, actual: Driver.BestScore.Number());
            Assert.AreEqual(expected: 0, actual: Driver.BestResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.BestResultGood.Number());
            Assert.AreEqual(expected: 0, actual: Driver.BestResultBad.Number());

            GameStart();
            Sleep(emurateNote1.JustTiming.TotalSeconds);
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);

            Sleep(Test1MusicTimeSecond - emurateNote1.JustTiming.TotalSeconds + 1);    // 1[s]余裕を持たせる
            Driver.EmurateGamePlayingResultOKButtonClick();
            Sleep(0.5);    // sleepなしだと失敗したことがある

            Driver.MusicList.ChangeSelectedIndex(Test1MusicIndex);
            Assert.AreEqual(expected: 2, actual: Driver.BestScore.Number());
            Assert.AreEqual(expected: 1, actual: Driver.BestResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.BestResultGood.Number());
            Assert.AreEqual(expected: 1, actual: Driver.BestResultBad.Number());
        }

        [TestMethod]
        public void TestSuspendAndRestartWhenGamePlaying()
        {
            EmurateNote emurateNote1 = new EmurateNote(PlayAreaX, PlayAreaY, 1);
            System.Windows.Point clickPointNote1 = emurateNote1.EmurateCalcJustJudgeLinePoint();

            GameStart(Test1MusicIndex);

            // 中断中のjudge無効確認のためにPerfectタイミングちょうどで中断
            Sleep(emurateNote1.JustTiming.TotalSeconds);
            Driver.EmurateSuspendGame();
            // TODO: statusのチェック、MainWindowのデバッグ用ではなく本番稼働のテキストを見るようにする
            //  -> メッセージ：後はMainWindow側を削除する
            Assert.IsTrue(Driver.PlayingMusicStatus.Contains("Suspend"));

            // この待機時間は適当でOK, 1秒も中断すれば以降の動作確認に問題なしと主観で判断
            Sleep(1);

            // 中断中のjudge無効を確認するための左クリック
            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultBad.Number());

            Driver.EmurateRestartGame();
            Assert.IsFalse(Driver.PlayingMusicStatus.Contains("Playing"));
            Sleep(UntilGameStartFromSuspend.TotalSeconds);
            Assert.IsTrue(Driver.PlayingMusicStatus.Contains("Playing"));

            Driver.EmurateLeftClickGamePlaying(clickPointNote1);
            Assert.AreEqual(expected: 1, actual: Driver.PlayingResultPerfect.Number());
            Assert.AreEqual(expected: 0, actual: Driver.PlayingResultBad.Number());

            Sleep(Test1MusicTimeSecond - emurateNote1.JustTiming.TotalSeconds + 1);    // 1[s]余裕を持たせる
            Assert.AreEqual(expected: 1, actual: Driver.PlayingResultBad.Number());    // 見逃しBad判定機能の確認

            // 曲終了の確認
            Assert.IsTrue(Driver.PlayingMusicStatus.Contains("Finish"));
        }

        [TestMethod]
        public void TestEnableOfChangingNoteSpeedRate()
        {
            // プレイヤーは、ノートの落下速度の倍率を指定できる
            Driver.EmurateChangeSpeedRate(0.5);
            GameStart(Test1MusicIndex);

            Assert.AreEqual(expected: 2, actual: Driver.GetDisplayNotesNum(0));
            Sleep(Test1MusicTimeSecond);
            Assert.AreEqual(expected: 0, actual: Driver.GetDisplayNotesNum(0));    // 初期実装時、速度0.5倍のnoteが画面から消えなかったことがあった
            Sleep(1);    // 同期するための少しの余裕
            Driver.EmurateGamePlayingResultOKButtonClick();

            Sleep(1);    // 同期するための少しの余裕
            Driver.EmurateChangeSpeedRate(3.0);
            GameStart(Test1MusicIndex);

            Assert.AreEqual(expected: 0, actual: Driver.GetDisplayNotesNum(0));
            Sleep(Test1MusicTimeSecond);
            Assert.AreEqual(expected: 0, actual: Driver.GetDisplayNotesNum(0));    // 0.5倍で画面に残ったことがあったので一応テスト
        }

        private void GameStart(int musicIndex)
        {
            Driver.MusicList.ChangeSelectedIndex(musicIndex);
            GameStart();
        }

        private void GameStart()
        {
            Driver.GameStartButton.Click();
            Sleep(UntilGameStart.TotalSeconds);
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

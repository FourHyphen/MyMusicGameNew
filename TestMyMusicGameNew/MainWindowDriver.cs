using Microsoft.VisualStudio.TestTools.UnitTesting;
using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using System.Reflection.Emit;
using System.Windows.Documents;
using Codeer.Friendly.Dynamic;
using System;

namespace TestMyMusicGameNew
{
    public partial class TestFeature
    {
        public class MainWindowDriver
        {
            // 個人的メモ：本来はCodeer.Friendly APIに依存しないようインタフェースでラップすべきだが、UIテスト可能なAPIを他に知らないのでこのままにする
            private dynamic MainWindow { get; }
            private IWPFDependencyObjectCollection<System.Windows.DependencyObject> Tree { get; set; }
            public MusicListAdapter MusicList { get; }
            public ButtonAdapter GameStartButton { get; }
            public LabelAdapter PlayingMusicStatus { get; }
            public LabelAdapter NotesNum { get; }
            private DisplayNotesAdapter DisplayNotes { get; }
            public LabelAdapter PlayingResultPerfect { get; }
            public LabelAdapter PlayingResultBad { get; }
            public LabelAdapter FinishResultPerfect { get; }
            public LabelAdapter FinishResultBad { get; }
            public LabelAdapter BestScore { get; }
            public LabelAdapter BestResultPerfect { get; }
            public LabelAdapter BestResultGood { get; }
            public LabelAdapter BestResultBad { get; }

            public MainWindowDriver(dynamic mainWindow)
            {
                MainWindow = mainWindow;
                Tree = new WindowControl(mainWindow).LogicalTree();
                MusicList = new MusicListAdapter(Tree, "MusicListBox");
                GameStartButton = new ButtonAdapter(Tree, "GameStartButton");
                PlayingMusicStatus = new LabelAdapter(Tree, "GameStatus");
                NotesNum = new LabelAdapter(Tree, "NotesNum");
                DisplayNotes = new DisplayNotesAdapter("Note");
                PlayingResultPerfect = new LabelAdapter(Tree, "ResultPerfect");
                PlayingResultBad = new LabelAdapter(Tree, "ResultBad");
                FinishResultPerfect = new LabelAdapter(Tree, "ResultFinishPerfect");
                FinishResultBad = new LabelAdapter(Tree, "ResultFinishBad");
                BestScore = new LabelAdapter(Tree, "BestScore");
                BestResultPerfect = new LabelAdapter(Tree, "BestResultPerfect");
                BestResultGood = new LabelAdapter(Tree, "BestResultGood");
                BestResultBad = new LabelAdapter(Tree, "BestResultBad");
            }

            /// <param name="startIndex">0始まり</param>
            /// <returns></returns>
            public int GetDisplayNotesNum(int startIndex)
            {
                Tree = new WindowControl(MainWindow).LogicalTree();  // 現在の画面状況を取得
                return DisplayNotes.GetDisplayNum(Tree, startIndex);
            }

            public void EmurateLeftClickGamePlaying(System.Windows.Point p)
            {
                MainWindow.PlayArea.Judge(p);
            }

            public void EmuratePressKeyboardGamePlaying(MyMusicGameNew.Keys.EnableKeys key)
            {
                MainWindow.PlayArea.ProcessKeyDown(key);
            }

            public void EmurateGamePlayingResultOKButtonClick()
            {
                MainWindow.PlayArea.DisplayMusicSelect();
            }

            public void ResetBestScore(int musicIndex)
            {
                MainWindow.MusicSelect.ResetBestScore(musicIndex);
            }

            public void EmurateSuspendGame()
            {
                MainWindow.PlayArea.ProcessKeyDown(MyMusicGameNew.Keys.EnableKeys.Suspend);
            }

            public void EmurateRestartGame()
            {
                MainWindow.PlayArea.ProcessKeyDown(MyMusicGameNew.Keys.EnableKeys.Restart);
            }

            public void EmurateChangeSpeedRate(double rate)
            {
                if (rate < 0.5 || rate > 3.0)
                {
                    throw new Exception("0.5 <= rate <= 3.0");
                }

                MainWindow.DebugSetNoteSpeedRate(rate);
            }
        }
    }
}

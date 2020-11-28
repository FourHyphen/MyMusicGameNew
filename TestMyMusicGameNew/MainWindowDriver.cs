using Microsoft.VisualStudio.TestTools.UnitTesting;
using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using System.Reflection.Emit;
using System.Windows.Documents;
using Codeer.Friendly.Dynamic;

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
            public LabelAdapter GameStatus { get; }
            public ButtonAdapter GameStartButton { get; }
            public LabelAdapter PlayingMusicStatus { get; }
            public LabelAdapter NotesNum { get; }
            private DisplayNotesAdapter DisplayNotes { get; }
            private LabelAdapter DisplayNotesNearestJudgeLine { get; }
            private LineAdapter JudgeLine { get; }
            public LabelAdapter ResultPerfect { get; }
            public LabelAdapter ResultGood { get; }
            public LabelAdapter ResultBad { get; }

            public MainWindowDriver(dynamic mainWindow)
            {
                MainWindow = mainWindow;
                Tree = new WindowControl(mainWindow).LogicalTree();
                MusicList = new MusicListAdapter(Tree, "MusicListBox");
                GameStatus = new LabelAdapter(Tree, "GameStatus");
                GameStartButton = new ButtonAdapter(Tree, "GameStartButton");
                PlayingMusicStatus = new LabelAdapter(Tree, "PlayingMusicStatus");
                NotesNum = new LabelAdapter(Tree, "NotesNum");
                DisplayNotes = new DisplayNotesAdapter("Note");
                DisplayNotesNearestJudgeLine = new LabelAdapter(Tree, "DisplayNotesNearestJudgeLine");
                JudgeLine = new LineAdapter(Tree, "JudgeLine");
                ResultPerfect = new LabelAdapter(Tree, "ResultPerfect");
                ResultGood = new LabelAdapter(Tree, "ResultGood");
                ResultBad = new LabelAdapter(Tree, "ResultBad");
            }

            public int GetDisplayNotesNum()
            {
                Tree = new WindowControl(MainWindow).LogicalTree();  // 現在の画面状況を取得
                return DisplayNotes.GetDisplayNum(Tree);
            }

            public System.Windows.Point GetDisplayNotesNearestJudgeLine()
            {
                string str = DisplayNotesNearestJudgeLine.Content();
                int xStart = "(".Length;
                int xLength = 7;
                int x = int.Parse(str.Substring(xStart, xLength));

                int yStart = xStart + xLength + ", ".Length;
                int yLength = 7;
                int y = int.Parse(str.Substring(yStart, yLength));
                return new System.Windows.Point(x, y);
            }

            public bool ExistJudgeLine()
            {
                return (JudgeLine != null);
            }

            public void EmurateLeftClickGamePlaying(System.Windows.Point p)
            {
                MainWindow.Judge(p);
            }
        }
    }
}

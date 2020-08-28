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

            public MainWindowDriver(dynamic mainWindow)
            {
                MainWindow = mainWindow;
                Tree = new WindowControl(mainWindow).LogicalTree();
                MusicList = new MusicListAdapter(Tree, "MusicListBox");
                GameStatus = new LabelAdapter(Tree, "GameStatus");
                GameStartButton = new ButtonAdapter(Tree, "GameStartButton");
                PlayingMusicStatus = new LabelAdapter(Tree, "PlayingMusicStatus");
                NotesNum = new LabelAdapter(Tree, "NotesNum");
            }
        }
    }
}

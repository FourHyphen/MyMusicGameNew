using Microsoft.VisualStudio.TestTools.UnitTesting;
using Codeer.Friendly.Windows.Grasp;
using RM.Friendly.WPFStandardControls;
using System.Reflection;
using System;
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
            public GameStatusAdapter GameStatus { get; }
            public ButtonAdapter GameStartButton { get; }
            public MainWindowDriver(dynamic mainWindow)
            {
                MainWindow = mainWindow;
                Tree = new WindowControl(mainWindow).LogicalTree();
                MusicList = new MusicListAdapter(Tree, "MusicListBox");
                GameStatus = new GameStatusAdapter(Tree, "GameStatus");
                GameStartButton = new ButtonAdapter(Tree, "GameStartButton");
            }
        }

        public class MusicListAdapter
        {
            private WPFListBox MusicList { get; }

            public MusicListAdapter(IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree, string listBoxName)
            {
                MusicList = GetMusicList(logicalTree, listBoxName);
            }

            private WPFListBox GetMusicList(IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree, string listBoxName)
            {
                try
                {
                    return new WPFListBox(logicalTree.ByType<System.Windows.Controls.ListBox>().ByName<System.Windows.Controls.ListBox>(listBoxName).Single());
                }
                catch (Exception)
                {
                    Failure(MethodBase.GetCurrentMethod().Name, listBoxName);
                    return null;
                }
            }

            public int Count()
            {
                return MusicList.ItemCount;
            }

            public void ChangeSelectedIndex(int index)
            {
                MusicList.EmulateChangeSelectedIndex(index);
            }

            private void Failure(string methodName, string elementName)
            {
                FailureGetElement("class " + this.GetType().Name + ", method " + methodName, elementName);
            }
        }

        public class GameStatusAdapter
        {
            private Codeer.Friendly.AppVar Instance { get; }

            public GameStatusAdapter(IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree, string labelName)
            {
                Instance = GetLabel(logicalTree, labelName);
            }

            public string Content()
            {
                return Instance.ToString().Replace("System.Windows.Controls.Label: ", "");
            }

            private Codeer.Friendly.AppVar GetLabel(IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree, string labelName)
            {
                var label = logicalTree.ByType<System.Windows.Controls.Label>().ByName(labelName).Single();
                if (label == null)
                {
                    Failure(MethodBase.GetCurrentMethod().Name, labelName);
                }
                return label;
            }

            private void Failure(string methodName, string elementName)
            {
                FailureGetElement("class " + this.GetType().Name + ", method " + methodName, elementName);
            }
        }

        public class ButtonAdapter
        {
            private WPFButtonBase GameStartButton { get; }

            public ButtonAdapter(IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree, string buttonName)
            {
                GameStartButton = GetButton(logicalTree, buttonName);
            }

            private WPFButtonBase GetButton(IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree, string buttonName)
            {
                try
                {
                    return new WPFButtonBase(logicalTree.ByType<System.Windows.Controls.Button>().ByName<System.Windows.Controls.Button>(buttonName).Single());
                }
                catch (Exception)
                {
                    Failure(MethodBase.GetCurrentMethod().Name, buttonName);
                    return null;
                }
            }

            public void Click()
            {
                GameStartButton.EmulateClick();
            }

            private void Failure(string methodName, string elementName)
            {
                FailureGetElement("class " + this.GetType().Name + ", method " + methodName, elementName);
            }
        }

        private static void FailureGetElement(string where, string elementName)
        {
            Assert.Fail(where + " Error: \"" + elementName + "\" get failed");
        }
    }
}

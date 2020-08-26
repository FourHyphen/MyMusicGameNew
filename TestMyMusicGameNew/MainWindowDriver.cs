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
            private dynamic MainWindow { get; }
            private IWPFDependencyObjectCollection<System.Windows.DependencyObject> Tree { get; set; }
            public MusicListAdapter MusicList { get; }
            public MainWindowDriver(dynamic mainWindow)
            {
                MainWindow = mainWindow;
                Tree = new WindowControl(mainWindow).LogicalTree();
                MusicList = new MusicListAdapter(Tree, "MusicListBox");
            }
        }

        public class MusicListAdapter
        {
            // 個人的メモ：本来はCodeer.Friendly APIに依存しないようインタフェースでラップすべきだが、UIテスト可能なAPIを他に知らないのでこのままにする
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

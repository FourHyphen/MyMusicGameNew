using RM.Friendly.WPFStandardControls;
using System.Reflection;
using System;

namespace TestMyMusicGameNew
{
    public partial class TestFeature
    {
        public class MusicListAdapter : DisplayControl
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
        }
    }
}

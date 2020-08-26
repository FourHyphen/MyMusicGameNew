using RM.Friendly.WPFStandardControls;
using System.Reflection;

namespace TestMyMusicGameNew
{
    public partial class TestFeature
    {
        public class GameStatusAdapter : DisplayControl
        {
            private Codeer.Friendly.AppVar Instance { get; }

            public GameStatusAdapter(IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree, string labelName)
            {
                Instance = GetLabel(logicalTree, labelName);
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

            public string Content()
            {
                return Instance.ToString().Replace("System.Windows.Controls.Label: ", "");
            }
        }
    }
}

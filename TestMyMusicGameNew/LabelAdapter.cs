using RM.Friendly.WPFStandardControls;
using System;
using System.Reflection;

namespace TestMyMusicGameNew
{
    public partial class TestFeature
    {
        public class LabelAdapter : DisplayControl
        {
            private Codeer.Friendly.AppVar Instance { get; }

            public LabelAdapter(IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree, string labelName)
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

            public bool Contains(string str)
            {
                return Content().Contains(str);
            }

            public int Number()
            {
                string str = Content();
                try
                {
                    return int.Parse(str);
                }
                catch (Exception ex)
                {
                    string exStr = "Label.Content = {" + str + "} の数値変換エラー";
                    throw new Exception(exStr, ex);
                }
            }
        }
    }
}

using RM.Friendly.WPFStandardControls;
using System.Reflection;
using System;

namespace TestMyMusicGameNew
{
    public partial class TestFeature
    {
        public class ButtonAdapter : DisplayControl
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
        }
    }
}

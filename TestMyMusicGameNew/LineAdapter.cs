using RM.Friendly.WPFStandardControls;
using System.Reflection;

namespace TestMyMusicGameNew
{
    public class LineAdapter : DisplayControl
    {
        private Codeer.Friendly.AppVar Instance { get; }

        public LineAdapter(IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree, string lineName)
        {
            Instance = GetLine(logicalTree, lineName);
        }

        private Codeer.Friendly.AppVar GetLine(IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree, string lineName)
        {
            var line = logicalTree.ByType<System.Windows.Shapes.Line>().ByName(lineName).Single();
            if (line == null)
            {
                Failure(MethodBase.GetCurrentMethod().Name, lineName);
            }
            return line;
        }
    }
}

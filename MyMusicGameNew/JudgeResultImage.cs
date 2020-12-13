using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyMusicGameNew
{
    public class JudgeResultImage
    {
        private GridPlayArea _GridPlayArea { get; set; }

        private JudgeResultImageSource PerfectImage { get; set; }

        private JudgeResultImageSource GoodImage { get; set; }

        private JudgeResultImageSource BadImage { get; set; }

        private static readonly string PerfectImagePath = "./GameData/JudgeResult/Result_Perfect.gif";

        private static readonly string GoodImagePath = "./GameData/JudgeResult/Result_Good.gif";

        private static readonly string BadImagePath = "./GameData/JudgeResult/Result_Bad.gif";

        private static readonly string PerfectResultName = "Perfect";

        private static readonly string GoodResultName = "Good";

        private static readonly string BadResultName = "Bad";

        public JudgeResultImage(GridPlayArea playArea, System.Windows.Point judgeResultImageCenter)
        {
            _GridPlayArea = playArea;
            Init(judgeResultImageCenter);
        }

        private void Init(System.Windows.Point judgeResultImageCenter)
        {
            PerfectImage = new JudgeResultImageSource(_GridPlayArea, PerfectResultName, PerfectImagePath, judgeResultImageCenter);
            GoodImage = new JudgeResultImageSource(_GridPlayArea, GoodResultName, GoodImagePath, judgeResultImageCenter);
            BadImage = new JudgeResultImageSource(_GridPlayArea, BadResultName, BadImagePath, judgeResultImageCenter);
        }

        public void Show(NoteJudge.JudgeType result)
        {
            RemoveShowingJudgeResultImage();
            JudgeResultImageSource image = GetWillShowImage(result);
            image.Show(_GridPlayArea);
        }

        private JudgeResultImageSource GetWillShowImage(NoteJudge.JudgeType result)
        {
            if (result == NoteJudge.JudgeType.Perfect)
            {
                return PerfectImage;
            }
            else if (result == NoteJudge.JudgeType.Good)
            {
                return GoodImage;
            }
            else
            {
                return BadImage;
            }
        }

        private void RemoveShowingJudgeResultImage()
        {
            PerfectImage.RemoveShowingJudgeResultImage(_GridPlayArea);
            GoodImage.RemoveShowingJudgeResultImage(_GridPlayArea);
            BadImage.RemoveShowingJudgeResultImage(_GridPlayArea);
        }
    }
}

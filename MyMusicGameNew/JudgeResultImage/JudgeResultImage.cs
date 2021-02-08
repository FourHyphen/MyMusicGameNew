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

        private DisplayImagePeriod PerfectImage { get; set; }

        private DisplayImagePeriod GoodImage { get; set; }

        private DisplayImagePeriod BadImage { get; set; }

        private static readonly string ImageDirPath = "./GameData/JudgeResult/";

        private static readonly string PerfectImagePath = ImageDirPath + "Result_Perfect.gif";

        private static readonly string GoodImagePath = ImageDirPath + "Result_Good.gif";

        private static readonly string BadImagePath = ImageDirPath + "Result_Bad.gif";

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
            PerfectImage = new DisplayImagePeriod(_GridPlayArea, PerfectResultName, PerfectImagePath, judgeResultImageCenter);
            GoodImage = new DisplayImagePeriod(_GridPlayArea, GoodResultName, GoodImagePath, judgeResultImageCenter);
            BadImage = new DisplayImagePeriod(_GridPlayArea, BadResultName, BadImagePath, judgeResultImageCenter);
        }

        public void Show(NoteJudge.JudgeType result)
        {
            RemoveShowingJudgeResultImage();
            DisplayImagePeriod image = GetWillShowImage(result);
            image.Show(_GridPlayArea);
        }

        private void RemoveShowingJudgeResultImage()
        {
            PerfectImage.RemoveShowingJudgeResultImage(_GridPlayArea);
            GoodImage.RemoveShowingJudgeResultImage(_GridPlayArea);
            BadImage.RemoveShowingJudgeResultImage(_GridPlayArea);
        }

        private DisplayImagePeriod GetWillShowImage(NoteJudge.JudgeType result)
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
    }
}

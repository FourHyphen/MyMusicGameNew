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

        public JudgeResultImage(GridPlayArea playArea, System.Windows.Point judgeResultImageCenter)
        {
            _GridPlayArea = playArea;
            Init(judgeResultImageCenter);
        }

        private void Init(System.Windows.Point judgeResultImageCenter)
        {
            PerfectImage = new DisplayImagePeriod(_GridPlayArea, PerfectImagePath, judgeResultImageCenter);
            GoodImage = new DisplayImagePeriod(_GridPlayArea, GoodImagePath, judgeResultImageCenter);
            BadImage = new DisplayImagePeriod(_GridPlayArea, BadImagePath, judgeResultImageCenter);
        }

        public void Show(NoteJudge.JudgeType result)
        {
            RemoveShowingJudgeResultImage();
            DisplayImagePeriod image = GetWillShowImage(result);
            image.Show(_GridPlayArea);
        }

        private void RemoveShowingJudgeResultImage()
        {
            PerfectImage.RemoveShowingImage(_GridPlayArea);
            GoodImage.RemoveShowingImage(_GridPlayArea);
            BadImage.RemoveShowingImage(_GridPlayArea);
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

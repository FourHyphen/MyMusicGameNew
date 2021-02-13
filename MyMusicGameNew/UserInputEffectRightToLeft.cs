using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    class UserInputEffectRightToLeft : UserInputEffect
    {
        private static readonly string EffectImagePath = ImageDirPath + "EffectRightToLeft.png";

        public UserInputEffectRightToLeft(GridPlayArea gridPlayArea) : base(gridPlayArea)
        {

        }

        public override void Init(GamePlayingArea gamePlayingArea)
        {
            EffectLine1 = new DisplayImagePeriod(_GridPlayArea, EffectImagePath, GetPointLine1(gamePlayingArea));
            EffectLine2 = new DisplayImagePeriod(_GridPlayArea, EffectImagePath, GetPointLine2(gamePlayingArea));
        }

        private System.Windows.Point GetPointLine1(GamePlayingArea gamePlayingArea)
        {
            double x = gamePlayingArea.GetLinePointXRightToLeft();
            double y = gamePlayingArea.GetLinePointYRightToLeft(1);
            return new System.Windows.Point(x, y);
        }

        private System.Windows.Point GetPointLine2(GamePlayingArea gamePlayingArea)
        {
            double x = gamePlayingArea.GetLinePointXRightToLeft();
            double y = gamePlayingArea.GetLinePointYRightToLeft(2);
            return new System.Windows.Point(x, y);
        }
    }
}

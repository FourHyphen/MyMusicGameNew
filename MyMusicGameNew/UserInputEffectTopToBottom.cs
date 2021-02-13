using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    class UserInputEffectTopToBottom : UserInputEffect
    {
        private static readonly string EffectImagePath = ImageDirPath + "EffectTopToBottom.png";

        public UserInputEffectTopToBottom(GridPlayArea gridPlayArea) : base(gridPlayArea)
        {
        }

        public override void Init(GamePlayingArea gamePlayingArea)
        {
            EffectLine1 = new DisplayImagePeriod(_GridPlayArea, EffectImagePath, GetPointLine1(gamePlayingArea));
            EffectLine2 = new DisplayImagePeriod(_GridPlayArea, EffectImagePath, GetPointLine2(gamePlayingArea));
        }

        private System.Windows.Point GetPointLine1(GamePlayingArea gamePlayingArea)
        {
            double x = gamePlayingArea.GetLinePointXTopToBottom(1);
            double y = gamePlayingArea.GetLinePointYTopToBottom();
            return new System.Windows.Point(x, y);
        }

        private System.Windows.Point GetPointLine2(GamePlayingArea gamePlayingArea)
        {
            double x = gamePlayingArea.GetLinePointXTopToBottom(2);
            double y = gamePlayingArea.GetLinePointYTopToBottom();
            return new System.Windows.Point(x, y);
        }
    }
}

using System;

namespace MyMusicGameNew
{
    public class UserInputEffect
    {
        private GridPlayArea _GridPlayArea { get; set; }

        private DisplayImagePeriod EffectLine1 { get; set; }

        private DisplayImagePeriod EffectLine2 { get; set; }

        private static readonly string ImageDirPath = "./GameData/UserInputEffect/";

        private static readonly string EffectImagePath = ImageDirPath + "Effect1.png";

        public UserInputEffect(GridPlayArea gridPlayArea, GamePlayingArea gamePlayingArea)
        {
            _GridPlayArea = gridPlayArea;
            Init(gamePlayingArea);
        }

        private void Init(GamePlayingArea gamePlayingArea)
        {
            EffectLine1 = new DisplayImagePeriod(_GridPlayArea, EffectImagePath, GetPointLine1(gamePlayingArea));
            EffectLine2 = new DisplayImagePeriod(_GridPlayArea, EffectImagePath, GetPointLine2(gamePlayingArea));
        }

        private System.Windows.Point GetPointLine1(GamePlayingArea gamePlayingArea)
        {
            // メッセージ：位置がleft, top方向にズレる -> 多分 DisplayImagePeriod の位置計算を共通化できてないか、JudgeLineYFromTop = 462だったから座標系が違うか、かな？
            double x = gamePlayingArea.GetLinePointX(1);
            double y = gamePlayingArea.GetLinePointY();
            return new System.Windows.Point(x, y);
        }

        private System.Windows.Point GetPointLine2(GamePlayingArea gamePlayingArea)
        {
            double x = gamePlayingArea.GetLinePointX(2);
            double y = gamePlayingArea.GetLinePointY();
            return new System.Windows.Point(x, y);
        }

        public void Show(int inputLine)
        {
            if (inputLine == 1)
            {
                EffectLine1.Show(_GridPlayArea);
            }
            else if (inputLine == 2)
            {
                EffectLine2.Show(_GridPlayArea);
            }
        }
    }
}
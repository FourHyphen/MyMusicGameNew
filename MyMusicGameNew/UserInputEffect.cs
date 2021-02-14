using System;

namespace MyMusicGameNew
{
    public class UserInputEffect
    {
        protected GridPlayArea _GridPlayArea { get; set; }

        protected DisplayImagePeriod EffectLine1 { get; set; }

        protected DisplayImagePeriod EffectLine2 { get; set; }

        protected static readonly string ImageDirPath = "./GameData/UserInputEffect/";

        private static readonly string EffectRightToLeftImagePath = ImageDirPath + "EffectRightToLeft.png";

        private static readonly string EffectTopToBottomImagePath = ImageDirPath + "EffectTopToBottom.png";

        private string EffectImagePath { get; } = "";

        private UserInputEffect(GridPlayArea gridPlayArea, string effectImagePath)
        {
            _GridPlayArea = gridPlayArea;
            EffectImagePath = effectImagePath;
        }

        public static UserInputEffect Create(GridPlayArea gridPlayArea, GamePlayingArea gamePlayingArea, GamePlaying.NoteDirection noteDirection)
        {
            UserInputEffect ret = null;
            if (noteDirection == GamePlaying.NoteDirection.RightToLeft)
            {
                ret = new UserInputEffect(gridPlayArea, EffectRightToLeftImagePath);
            }
            else
            {
                ret = new UserInputEffect(gridPlayArea, EffectTopToBottomImagePath);
            }

            ret.Init(gamePlayingArea);
            return ret;
        }

        private void Init(GamePlayingArea gamePlayingArea)
        {
            EffectLine1 = new DisplayImagePeriod(_GridPlayArea, EffectImagePath, GetPointLine1(gamePlayingArea));
            EffectLine2 = new DisplayImagePeriod(_GridPlayArea, EffectImagePath, GetPointLine2(gamePlayingArea));
        }

        private System.Windows.Point GetPointLine1(GamePlayingArea gamePlayingArea)
        {
            return gamePlayingArea.GetLinePoint(1);
        }

        private System.Windows.Point GetPointLine2(GamePlayingArea gamePlayingArea)
        {
            return gamePlayingArea.GetLinePoint(2);
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
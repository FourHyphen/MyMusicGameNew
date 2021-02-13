using System;

namespace MyMusicGameNew
{
    public class UserInputEffect
    {
        private GridPlayArea _GridPlayArea { get; set; }

        private DisplayImagePeriod EffectLine1 { get; set; }

        private DisplayImagePeriod EffectLine2 { get; set; }

        private static readonly string ImageDirPath = "./GameData/UserInputEffect/";

        private static readonly string EffectTopToBottomImagePath = ImageDirPath + "EffectTopToBottom.png";

        private static readonly string EffectRightToLeftImagePath = ImageDirPath + "EffectRightToLeft.png";

        public UserInputEffect(GridPlayArea gridPlayArea, GamePlayingArea gamePlayingArea, GamePlaying.NoteDirection noteDirection)
        {
            _GridPlayArea = gridPlayArea;
            Init(gamePlayingArea, noteDirection);
        }

        private void Init(GamePlayingArea gamePlayingArea, GamePlaying.NoteDirection noteDirection)
        {
            if (noteDirection == GamePlaying.NoteDirection.RightToLeft)
            {
                InitRightToLeft(gamePlayingArea);
            }
            else
            {
                InitTopToBottom(gamePlayingArea);
            }
        }

        private void InitRightToLeft(GamePlayingArea gamePlayingArea)
        {
            EffectLine1 = new DisplayImagePeriod(_GridPlayArea, EffectRightToLeftImagePath, GetPointLine1RightToLeft(gamePlayingArea));
            EffectLine2 = new DisplayImagePeriod(_GridPlayArea, EffectRightToLeftImagePath, GetPointLine2RightToLeft(gamePlayingArea));
        }

        private System.Windows.Point GetPointLine1RightToLeft(GamePlayingArea gamePlayingArea)
        {
            double x = gamePlayingArea.GetLinePointXRightToLeft();
            double y = gamePlayingArea.GetLinePointYRightToLeft(1);
            return new System.Windows.Point(x, y);
        }

        private System.Windows.Point GetPointLine2RightToLeft(GamePlayingArea gamePlayingArea)
        {
            double x = gamePlayingArea.GetLinePointXRightToLeft();
            double y = gamePlayingArea.GetLinePointYRightToLeft(2);
            return new System.Windows.Point(x, y);
        }

        private void InitTopToBottom(GamePlayingArea gamePlayingArea)
        {
            EffectLine1 = new DisplayImagePeriod(_GridPlayArea, EffectTopToBottomImagePath, GetPointLine1TopToBottom(gamePlayingArea));
            EffectLine2 = new DisplayImagePeriod(_GridPlayArea, EffectTopToBottomImagePath, GetPointLine2TopToBottom(gamePlayingArea));
        }

        private System.Windows.Point GetPointLine1TopToBottom(GamePlayingArea gamePlayingArea)
        {
            double x = gamePlayingArea.GetLinePointXTopToBottom(1);
            double y = gamePlayingArea.GetLinePointYTopToBottom();
            return new System.Windows.Point(x, y);
        }

        private System.Windows.Point GetPointLine2TopToBottom(GamePlayingArea gamePlayingArea)
        {
            double x = gamePlayingArea.GetLinePointXTopToBottom(2);
            double y = gamePlayingArea.GetLinePointYTopToBottom();
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
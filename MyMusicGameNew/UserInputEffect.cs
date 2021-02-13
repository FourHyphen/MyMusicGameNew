using System;

namespace MyMusicGameNew
{
    public abstract class UserInputEffect
    {
        protected GridPlayArea _GridPlayArea { get; set; }

        protected DisplayImagePeriod EffectLine1 { get; set; }

        protected DisplayImagePeriod EffectLine2 { get; set; }

        protected static readonly string ImageDirPath = "./GameData/UserInputEffect/";

        protected UserInputEffect(GridPlayArea gridPlayArea)
        {
            _GridPlayArea = gridPlayArea;
        }

        public static UserInputEffect Create(GridPlayArea gridPlayArea, GamePlayingArea gamePlayingArea, GamePlaying.NoteDirection noteDirection)
        {
            UserInputEffect ret = null;
            if (noteDirection == GamePlaying.NoteDirection.RightToLeft)
            {
                ret = new UserInputEffectRightToLeft(gridPlayArea);
            }
            else
            {
                ret = new UserInputEffectTopToBottom(gridPlayArea);
            }

            ret.Init(gamePlayingArea);
            return ret;
        }

        public abstract void Init(GamePlayingArea gamePlayingArea);

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
namespace MyMusicGameNew
{
    public class PlayingMusicFactory
    {
        public static PlayingMusic Create(string musicDataPath, GridPlayArea playArea, bool isTest=false)
        {
            if (isTest)
            {
                return new PlayingMusicFake(musicDataPath);
            }
            else
            {
                return new PlayingMusicWav(musicDataPath, playArea);
            }
        }
    }
}

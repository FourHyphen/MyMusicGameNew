namespace MyMusicGameNew
{
    class PlayingMusicFake : PlayingMusic
    {
        public PlayingMusicFake(string dataPath) : base(dataPath) { }

        public override void PlayAsync()
        {
            // not play music
        }

        public override void Stop()
        {
            // not play music
        }

        public override void Restart()
        {
            // not play music
        }
    }
}

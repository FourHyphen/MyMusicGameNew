namespace MyMusicGameNew
{
    public abstract class PlayingMusic
    {
        private string _DataPath { get; }

        protected string DataPath
        {
            get
            {
                return Common.GetFilePathOfDependentEnvironment(_DataPath);
            }
        }

        public PlayingMusic(string dataPath)
        {
            _DataPath = dataPath;
        }

        public abstract void PlayAsync();

        public abstract void Stop();

        public abstract void Restart();
    }
}

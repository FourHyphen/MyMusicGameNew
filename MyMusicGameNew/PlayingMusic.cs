using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public abstract class PlayingMusic
    {
        protected string DataPath { get; }

        public PlayingMusic(string dataPath)
        {
            DataPath = dataPath;
        }

        public abstract void PlayAsync();

        protected string GetMusicFileFullPath()
        {
            return Common.GetFilePathOfDependentEnvironment(DataPath);
        }
    }
}

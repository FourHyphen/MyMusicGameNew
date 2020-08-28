using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public abstract class PlayingMusic
    {
        protected string MusicName { get; }

        public PlayingMusic(string musicName)
        {
            MusicName = musicName;
        }

        public abstract void PlayAsync();

        protected string GetMusicFilePathWithoutExtension()
        {
            return Common.GetFilePathOfDependentEnvironment("/GameData/Music/" + MusicName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyMusicGameNew
{
    public abstract class PlayingMusic
    {
        private string _DataPath { get; }

        protected string DataPath
        {
            get
            {
                return System.IO.Path.GetFullPath(_DataPath);
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

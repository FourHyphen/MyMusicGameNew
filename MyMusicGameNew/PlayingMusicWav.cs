using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class PlayingMusicWav : PlayingMusic
    {
        public PlayingMusicWav(string dataPath) : base(dataPath) { }

        public override void PlayAsync()
        {
            System.Media.SoundPlayer Player = new System.Media.SoundPlayer(GetMusicFileFullPath());
            Player.Play();
        }
    }
}

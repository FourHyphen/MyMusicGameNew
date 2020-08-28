using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    class PlayingMusicFake : PlayingMusic
    {
        public PlayingMusicFake(string musicName) : base(musicName) { }

        public override void PlayAsync()
        {
            // not play music
        }
    }
}

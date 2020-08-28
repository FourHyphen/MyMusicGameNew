using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class PlayingMusicWav : PlayingMusic
    {
        public PlayingMusicWav(string musicName) : base(musicName) { }

        public override void PlayAsync()
        {
            string wavFilePath = GetMusicFilePathWithoutExtension() + ".wav";
            System.Media.SoundPlayer Player = new System.Media.SoundPlayer(wavFilePath);
            Player.Play();
        }
    }
}

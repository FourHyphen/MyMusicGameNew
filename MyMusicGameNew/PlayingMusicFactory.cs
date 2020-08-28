using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class PlayingMusicFactory
    {
        public static PlayingMusic Create(string musicName, bool isTest=false)
        {
            if (isTest)
            {
                return new PlayingMusicFake(musicName);
            }
            else
            {
                return new PlayingMusicWav(musicName);
            }
        }
    }
}

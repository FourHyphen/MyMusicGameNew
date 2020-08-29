using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class Music
    {
        public string Name { get; }

        public int TimeSecond { get; }

        public List<Note> Notes { get; private set; }

        private PlayingMusic PlayingMusic { get; set; }

        public Music(string name, int timeSecond, List<Note> notes, PlayingMusic play)
        {
            Name = name;
            TimeSecond = timeSecond;
            Notes = notes;
            PlayingMusic = play;
        }

        public void PlayAsync()
        {
            PlayingMusic.PlayAsync();
        }
    }
}

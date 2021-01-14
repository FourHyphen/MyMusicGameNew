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

        private MusicBestResult BestResult { get; set; }

        public int NotesNum { get { return Notes.Count; } }

        public int BestScore { get { return BestResult.BestScore; } }

        public int BestResultPerfect { get { return BestResult.BestResultPerfect; } }

        public int BestResultGood { get { return BestResult.BestResultGood; } }

        public int BestResultBad { get { return BestResult.BestResultBad; } }

        public Music(string name, int timeSecond, List<Note> notes, PlayingMusic play)
        {
            Name = name;
            TimeSecond = timeSecond;
            Notes = notes;
            PlayingMusic = play;
            InitMusicNoteImage();
            InitBestResult();
        }

        private void InitMusicNoteImage()
        {
            for (int i = 0; i < Notes.Count; i++)
            {
                Notes[i].InitImage(i);
            }
        }

        private void InitBestResult()
        {
            BestResult = MusicBestResult.Create(Name);
        }

        public void PlayAsync()
        {
            PlayingMusic.PlayAsync();
        }

        public Note GetLatestUnjudgedNoteForLine(int inputLine)
        {
            foreach (Note n in Notes)
            {
                if (n.AlreadyJudged())
                {
                    continue;
                }

                if (inputLine == n.XLine)
                {
                    return n;
                }
            }

            return null;
        }
    }
}

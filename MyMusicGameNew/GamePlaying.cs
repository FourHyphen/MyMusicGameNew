using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MyMusicGameNew
{
    public class GamePlaying : GameState
    {
        private Music Music { get; }

        private System.Diagnostics.Stopwatch GameTimer { get; set; }

        private System.Timers.Timer GameFinishTimer { get; set; }

        private Task DisplayingNotesTask { get; set; }

        private int PerfectNum { get; set; } = 0;

        private int GoodNum { get; set; } = 0;

        private int BadNum { get; set; } = 0;

        public GamePlaying(MainWindow main, Music music) : base(main)
        {
            Music = music;
            InitMusicNoteImage();
        }

        ~GamePlaying()
        {
            if (GameFinishTimer != null)
            {
                GameFinishTimer.Stop();
                GameFinishTimer.Dispose();
            }

            if (GameTimer != null)
            {
                GameTimer.Stop();
            }
        }

        public void Start()
        {
            var cts = new CancellationTokenSource();
            DisplayNotesCore(Music.Notes, new TimeSpan());
            DisplayInfo();
            SetGameFinishedTimer(Music.TimeSecond, cts);
            StartDisplayingNotes(Music.Notes, cts.Token);
            PlayMusic();
            StartGameTimer();
        }

        private void InitMusicNoteImage()
        {
            for (int i = 0; i < Music.Notes.Count; i++)
            {
                Music.Notes[i].InitImage(i);
            }
        }

        private void DisplayInfo()
        {
            SetGameStatus("Playing");
            SetPlayingMusicStatus("Playing");
            SetNotesNum();
        }

        private void SetNotesNum()
        {
            Main.NotesNum.Content = Music.Notes.Count.ToString();
        }

        private void PlayMusic()
        {
            Music.PlayAsync();
        }

        private void SetGameFinishedTimer(int musicTimeSecond, CancellationTokenSource cts)
        {
            GameFinishTimer = new System.Timers.Timer();
            GameFinishTimer.Interval = (musicTimeSecond + 1) * 1000;  // 1[s]余裕を持たせる
            GameFinishTimer.Elapsed += (s, e) =>
            {
                Main.Dispatcher.Invoke(new Action(() =>
                {
                    ProcessGameFinished(cts);
                }));
            };
            GameFinishTimer.Start();
        }

        private void ProcessGameFinished(CancellationTokenSource cts)
        {
            GameFinishTimer.Stop();
            GameFinishTimer.Dispose();
            StopDisplayingNotes(cts);
            SetGameStatus("Finished");
            SetPlayingMusicStatus("Finished");
        }

        private void StopDisplayingNotes(CancellationTokenSource cts)
        {
            cts.Cancel();
        }

        private void StartDisplayingNotes(List<Note> notes, CancellationToken ct)
        {
            Main.DisplayNotesNearestJudgeLine.Visibility = Visibility.Visible;
            DisplayingNotesTask = Task.Run(() =>
            {
                DisplayingNotes(notes, ct);
            });
        }

        private void DisplayingNotes(List<Note> notes, CancellationToken ct)
        {
            while (true)
            {
                Main.DisplayNotesNearestJudgeLine.Dispatcher.Invoke(new Action(() =>
                {
                    TimeSpan now = GameTimer.Elapsed;
                    DisplayNotesCore(notes, now);
                }));

                if (ct.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        private void DisplayNotesCore(List<Note> notes, TimeSpan now)
        {
            for (int i = 0; i < notes.Count; i++)
            {
                Note note = notes[i];

                note.CalcNowPoint(now);
                if (DoNoteNeedDisplayingForPlayArea(note))
                {
                    DisplayNotePlayArea(note);
                }

                SetTestString(note, i);
            }
        }

        private bool DoNoteNeedDisplayingForPlayArea(Note note)
        {
            // TODO：表示しなくてよいNoteをスキップする
            if (note.AlreadyJudged())
            {
                return false;
            }

            if (!note.IsInsidePlayArea())
            {
                return false;
            }

            return true;
        }

        private void DisplayNotePlayArea(Note note)
        {
            System.Windows.Controls.Image image = note.DisplayImage;
            if (!Main.PlayArea.Children.Contains(image))
            {
                note.SetVisible();
                Main.PlayArea.Children.Add(image);
            }
        }

        private void SetTestString(Note note, int index)
        {
            // テスト用: 現在座標の表示
            if (index == 0)
            {
                string x = ((int)note.NowX).ToString().PadLeft(7);
                string y = ((int)note.NowY).ToString().PadLeft(7);
                Main.DisplayNotesNearestJudgeLine.Content = "(" + x + ", " + y + ")";
            }
        }

        private void StartGameTimer()
        {
            GameTimer = new System.Diagnostics.Stopwatch();
            GameTimer.Start();
        }

        public void Judge(System.Windows.Point mouseClicked)
        {
            Note note = GetLatestUnjudgedNoteForLine(mouseClicked.X, mouseClicked.Y);
            if (note is null)
            {
                return;
            }

            TimeSpan now = GameTimer.Elapsed;
            note.Judge(now);
            if (note.AlreadyJudged())
            {
                RemoveNotePlayArea(note);
                UpdateJudgeResult(note.JudgeResult);
            }
        }

        private Note GetLatestUnjudgedNoteForLine(double x, double y)
        {
            foreach (Note n in Music.Notes)
            {
                if (!n.AlreadyJudged(x))
                {
                    return n;
                }
            }

            return null;
        }

        private void RemoveNotePlayArea(Note note)
        {
            System.Windows.Controls.Image image = note.DisplayImage;
            if (Main.PlayArea.Children.Contains(image))
            {
                Main.PlayArea.Children.Remove(image);
            }
        }

        private void UpdateJudgeResult(NoteJudge.JudgeType result)
        {
            if (result == NoteJudge.JudgeType.Perfect)
            {
                PerfectNum++;
                Main.ResultPerfect.Content = PerfectNum.ToString();
            }
            else if (result == NoteJudge.JudgeType.Good)
            {
                GoodNum++;
                Main.ResultGood.Content = GoodNum.ToString();
            }
            else if (result == NoteJudge.JudgeType.Bad)
            {
                BadNum++;
                Main.ResultBad.Content = BadNum.ToString();
            }
        }
    }
}

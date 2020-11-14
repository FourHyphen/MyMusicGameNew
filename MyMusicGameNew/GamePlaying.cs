using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyMusicGameNew
{
    public class GamePlaying : GameState
    {
        private Music Music { get; }

        private System.Diagnostics.Stopwatch GameTimer { get; set; }

        private System.Timers.Timer GameFinishTimer { get; set; }

        private Task DisplayingNotesTask { get; set; }

        private BitmapSource NotesBitmap { get; set; }

        public GamePlaying(MainWindow main, Music music) : base(main)
        {
            Music = music;
            InitNotesImage();
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

        private void InitNotesImage(string noteImagePath = "./GameData/NoteImage/note.png")
        {
            // TODO: Note画像を管理するクラスに分離
            NotesBitmap = Common.GetImage(noteImagePath);
        }

        public void Start()
        {
            var cts = new CancellationTokenSource();
            InitMusicNoteImage();
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
                Note n = Music.Notes[i];
                n.Image = CreateDisplayNotes(i);
            }
            DisplayNotesCore(Music.Notes, new TimeSpan());
        }

        private Image CreateDisplayNotes(int index)
        {
            Image noteImage = new Image();
            noteImage.Source = NotesBitmap.Clone();
            noteImage.Stretch = Stretch.None;
            noteImage.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            noteImage.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            noteImage.Name = "Note" + index.ToString();

            return noteImage;
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
                Note n = notes[i];

                // TODO：すでに判定済み等、表示しなくてよいNoteをスキップする
                AddNotesForPlayArea(n, now);

                // テスト用: 現在座標の表示
                if (i == 0)
                {
                    string x = ((int)n.NowX).ToString().PadLeft(7);
                    string y = ((int)n.NowY).ToString().PadLeft(7);
                    Main.DisplayNotesNearestJudgeLine.Content = "(" + x + ", " + y + ")";
                }
            }
        }

        private void AddNotesForPlayArea(Note note, TimeSpan now)
        {
            note.CalcNowPoint(now);
            if (note.IsInsidePlayArea())
            {
                AddNoteImageForPlayArea(note);
            }
        }

        private void AddNoteImageForPlayArea(Note note)
        {
            note.Image.RenderTransform = GetNotesTransform(note.NowX, note.NowY);
            if (!Main.PlayArea.Children.Contains(note.Image))
            {
                note.Image.Visibility = System.Windows.Visibility.Visible;
                Main.PlayArea.Children.Add(note.Image);
            }
        }

        private Transform GetNotesTransform(double x, double y)
        {
            var transform = new TransformGroup();
            transform.Children.Add(new TranslateTransform(x, y));

            return transform;
        }

        //private void RemoveNotesForPlayArea(Notes notes)
        //{
        //    if (PlayArea.Children.Contains(notes.Image))
        //    {
        //        PlayArea.Children.Remove(notes.Image);
        //    }
        //}

        private void StartGameTimer()
        {
            GameTimer = new System.Diagnostics.Stopwatch();
            GameTimer.Start();
        }
    }
}

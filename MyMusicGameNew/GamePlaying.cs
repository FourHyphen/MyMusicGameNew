﻿using System;
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
        private GamePlayingArea _GamePlayingArea { get; set; }

        private GamePlayingDisplay _GamePlayingDisplay { get; set; }

        private Music Music { get; }

        private System.Windows.Point JudgeResultDisplayCenterPosition { get; set; }

        private System.Diagnostics.Stopwatch GameTimer { get; set; }

        private System.Timers.Timer GameFinishTimer { get; set; }

        private Task TaskKeepMovingDuringGame { get; set; }

        public GamePlaying(MainWindow main, GamePlayingArea area, string musicName, bool IsTest) : base(main)
        {
            _GamePlayingArea = area;
            _GamePlayingDisplay = new GamePlayingDisplay(main, area.JudgeResultDisplayCenterPosition);
            Music = new MusicFactory().Create(musicName, isTest: IsTest);
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
            DisplayInfo();
            SetGameFinishedTimer(Music.TimeSecond, cts);
            StartTaskKeepMovingDuringGame(Music.Notes, cts.Token);
            PlayMusic();
            StartGameTimer();
        }

        private void InitMusicNoteImage()
        {
            // TODO: MusicFactoryにこの処理を移植した方が良い(内部処理を公開してしまってる)
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

        private void StartTaskKeepMovingDuringGame(List<Note> notes, CancellationToken ct)
        {
            Main.DisplayNotesNearestJudgeLine.Visibility = Visibility.Visible;
            TaskKeepMovingDuringGame = Task.Run(() =>
            {
                DisplayNotesAndCheckMissedNotes(notes, ct);
            });
        }

        private void DisplayNotesAndCheckMissedNotes(List<Note> notes, CancellationToken ct)
        {
            while (true)
            {
                Main.DisplayNotesNearestJudgeLine.Dispatcher.Invoke(new Action(() =>
                {
                    TimeSpan now = GameTimer.Elapsed;
                    DisplayNotesAndCheckMissedNotesCore(notes, now);
                }));

                if (ct.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        private void DisplayNotesAndCheckMissedNotesCore(List<Note> notes, TimeSpan now)
        {
            for (int i = 0; i < notes.Count; i++)
            {
                Note note = notes[i];
                if (note.AlreadyJudged())
                {
                    continue;
                }

                JudgeBadWhenNotePassedJudgeLineForAWhile(note, now);
                DisplayNote(note, now);
                _GamePlayingDisplay.SetTestString(note, i);
            }
        }

        private void JudgeBadWhenNotePassedJudgeLineForAWhile(Note note, TimeSpan now)
        {
            note.JudgeBadWhenNotePassedJudgeLineForAWhile(now);
            if (note.AlreadyJudged())
            {
                _GamePlayingDisplay.DisplayNoteJudgeResult(note);
            }
        }

        private void DisplayNote(Note note, TimeSpan now)
        {
            note.CalcNowPoint(_GamePlayingArea, now);
            if (_GamePlayingArea.IsInsidePlayArea(note))
            {
                _GamePlayingDisplay.DisplayNotePlayArea(note);
            }
        }

        private void StartGameTimer()
        {
            GameTimer = new System.Diagnostics.Stopwatch();
            GameTimer.Start();
        }

        public void Judge(System.Windows.Point mouseClicked)
        {
            Note note = GetLatestUnjudgedNoteForLine(mouseClicked);
            if (note is null)
            {
                return;
            }

            TimeSpan now = GameTimer.Elapsed;
            note.Judge(now);
            if (note.AlreadyJudged())
            {
                _GamePlayingDisplay.DisplayNoteJudgeResult(note);
            }
        }

        private Note GetLatestUnjudgedNoteForLine(System.Windows.Point input)
        {
            foreach (Note n in Music.Notes)
            {
                if (n.AlreadyJudged())
                {
                    continue;
                }

                int inputLine = _GamePlayingArea.ConvertXLine(input.X);
                if (inputLine == n.XLine)
                {
                    return n;
                }
            }

            return null;
        }
    }
}

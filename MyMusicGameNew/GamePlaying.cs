﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MyMusicGameNew
{
    public class GamePlaying
    {
        private GridPlayArea _GridPlayArea { get; set; }

        private GamePlayingArea _GamePlayingArea { get; set; }

        private GamePlayingDisplay _GamePlayingDisplay { get; set; }

        private Music Music { get; }

        private PlayingMusic PlayingMusic { get; set; }

        private NoteSE NoteSE { get; }

        private System.Diagnostics.Stopwatch GameTimer { get; set; }

        private System.Timers.Timer GameFinishTimer { get; set; }

        private CancellationTokenSource ToCallGameFinish { get; set; }

        public GamePlaying(GridPlayArea playArea, Music music, double noteSpeedRate, bool IsTest)
        {
            _GridPlayArea = playArea;
            _GamePlayingArea = new GamePlayingArea((int)playArea.PlayArea.ActualWidth, (int)playArea.PlayArea.ActualHeight, noteSpeedRate);
            _GamePlayingDisplay = new GamePlayingDisplay(playArea, music.NotesNum);
            Music = music;
            InitPlayingMusic(Music.GetMusicDataPath(), playArea, IsTest);
            NoteSE = new NoteSEFactory().Create(IsTest);
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

        private void InitPlayingMusic(string musicDataPath, GridPlayArea playArea, bool isTest)
        {
            PlayingMusic = PlayingMusicFactory.Create(musicDataPath, playArea, isTest);
        }

        public void Starting()
        {
            // TODO: 待機時間の外部管理化
            GameInit();
            _GamePlayingDisplay.DisplayStartingWait(3);
        }

        #region private: ゲーム開始直前の処理の詳細

        private void GameInit()
        {
            GameTimer = new System.Diagnostics.Stopwatch();
            StartProcessingGame(Music.TimeMilliSecond);
        }

        private void StartProcessingGame(double musicRemainingTimeMilliSecond)
        {
            if (ToCallGameFinish != null)
            {
                ToCallGameFinish.Dispose();
            }
            ToCallGameFinish = new CancellationTokenSource();
            InitGameFinishedTimer(musicRemainingTimeMilliSecond);
            StartTaskKeepMovingDuringGame();
        }

        // ゲーム終了時の処理
        private void InitGameFinishedTimer(double musicTimeMilliSecond)
        {
            GameFinishTimer = new System.Timers.Timer();
            GameFinishTimer.Interval = musicTimeMilliSecond + 1000.0;  // 1[s]余裕を持たせる
            GameFinishTimer.Elapsed += (s, e) =>
            {
                _GridPlayArea.Dispatcher.Invoke(new Action(() =>
                {
                    ProcessGameFinished();
                }));
            };
        }

        private void ProcessGameFinished()
        {
            StopGame();

            _GamePlayingDisplay.GameFinish();
            _GridPlayArea.GameFinish();

            ResultSave();
        }

        private void StopGame()
        {
            GameTimer.Stop();
            PlayingMusic.Stop();

            GameFinishTimer.Stop();
            GameFinishTimer.Dispose();
            StopDisplayingNotes();
        }

        private void StopDisplayingNotes()
        {
            ToCallGameFinish.Cancel();
        }

        private void ResultSave()
        {
            Music.SaveBestScore();
        }

        // ゲーム中に常駐させる処理
        private void StartTaskKeepMovingDuringGame()
        {
            Task.Run(() =>
            {
                CheckMissedNotesAndDisplayNotes();
            });
        }

        private void CheckMissedNotesAndDisplayNotes()
        {
            CancellationToken ct = ToCallGameFinish.Token;
            while (true)
            {
                _GridPlayArea.JudgeLine.Dispatcher.Invoke(new Action(() =>
                {
                    TimeSpan now = GameTimer.Elapsed;
                    CheckMissedNotesAndDisplayNotesCore(now);
                }));

                if (ct.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        private void CheckMissedNotesAndDisplayNotesCore(TimeSpan now)
        {
            foreach (Note note in Music.Notes)
            {
                if (note.AlreadyJudged())
                {
                    continue;
                }

                note.JudgeBadWhenNotePassedJudgeLineForAWhile(now);
                if (note.AlreadyJudged())
                {
                    DisplayNoteJudgeResult(note);
                }
                else
                {
                    note.CalcNowPoint(_GamePlayingArea, now);
                    DisplayNote(note);
                }
            }
        }

        private void DisplayNote(Note note)
        {
            if (_GamePlayingArea.IsInsidePlayArea(note))
            {
                _GamePlayingDisplay.DisplayNotePlayArea(note);
            }
        }

        #endregion

        public void Start()
        {
            GameFinishTimer.Start();
            PlayingMusic.PlayAsync();
            GameTimer.Start();
        }

        public void Judge(Keys.EnableKeys key)
        {
            int inputLine = _GamePlayingArea.ConvertXLine(key);
            JudgeCore(inputLine);
        }

        public void Judge(System.Windows.Point mouseClicked)
        {
            int inputLine = _GamePlayingArea.ConvertXLine(mouseClicked.X);
            JudgeCore(inputLine);
        }

        #region private: ユーザー入力によるNoteのJudge、詳細

        private void JudgeCore(int inputLine)
        {
            Note note = Music.GetLatestUnjudgedNoteForLine(inputLine);
            if (note is null)
            {
                return;
            }

            TimeSpan now = GameTimer.Elapsed;
            note.Judge(now);
            if (note.AlreadyJudged())
            {
                NoteSE.Sound();
                DisplayNoteJudgeResult(note);
            }
        }

        private void DisplayNoteJudgeResult(Note note)
        {
            _GamePlayingDisplay.DisplayNoteJudgeResult(note);
            _GamePlayingDisplay.RemoveNotePlayArea(note);
        }

        #endregion

        public bool DoPlaying()
        {
            return GameTimer.IsRunning;
        }

        public void Suspend()
        {
            StopGame();
            _GamePlayingDisplay.DisplaySuspend();
        }

        public void Restart()
        {
            RestartCore();
        }

        private async void RestartCore()
        {
            // 時間のかかる処理をゲーム再開前に済ます
            double untilMusicFinishedTimeMilliSecond = Music.TimeMilliSecond - GameTimer.Elapsed.TotalMilliseconds;
            StartProcessingGame(untilMusicFinishedTimeMilliSecond);

            await Task.Run(() =>
            {
                // TODO 時間設定の外部管理化
                _GamePlayingDisplay.DisplayRestartWait(3);
            });

            GameFinishTimer.Start();
            PlayingMusic.Restart();
            GameTimer.Start();
        }
    }
}

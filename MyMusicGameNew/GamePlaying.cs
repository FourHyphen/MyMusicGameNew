using System;
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
        private MainWindow Main { get; set; }

        private GridPlayArea _GridPlayArea { get; set; }

        private GamePlayingArea _GamePlayingArea { get; set; }

        private GamePlayingDisplay _GamePlayingDisplay { get; set; }

        private Music Music { get; }

        private System.Diagnostics.Stopwatch GameTimer { get; set; }

        private System.Timers.Timer GameFinishTimer { get; set; }

        private Task TaskKeepMovingDuringGame { get; set; }

        public GamePlaying(MainWindow main, GridPlayArea playArea, Music music, bool IsTest)
        {
            Main = main;
            _GridPlayArea = playArea;
            _GamePlayingArea = new GamePlayingArea((int)playArea.PlayArea.ActualWidth, (int)playArea.PlayArea.ActualHeight);
            _GamePlayingDisplay = new GamePlayingDisplay(playArea);
            Music = music;
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

        public void Starting()
        {
            // TODO: 待機時間の外部管理化
            GameInit();
            DisplayInfo();
            _GamePlayingDisplay.DisplayStartingWait(3);
        }

        #region ゲーム開始直前の処理の詳細

        private void GameInit()
        {
            GameTimer = new System.Diagnostics.Stopwatch();

            var cts = new CancellationTokenSource();
            InitGameFinishedTimer(Music.TimeSecond, cts);
            StartTaskKeepMovingDuringGame(cts.Token);
        }

        // ゲーム終了時の処理
        private void InitGameFinishedTimer(int musicTimeSecond, CancellationTokenSource cts)
        {
            GameFinishTimer = new System.Timers.Timer();
            GameFinishTimer.Interval = (musicTimeSecond + 1) * 1000;  // 1[s]余裕を持たせる
            GameFinishTimer.Elapsed += (s, e) =>
            {
                _GridPlayArea.Dispatcher.Invoke(new Action(() =>
                {
                    ProcessGameFinished(cts);
                }));
            };
        }

        private void ProcessGameFinished(CancellationTokenSource cts)
        {
            GameFinishTimer.Stop();
            GameFinishTimer.Dispose();
            StopDisplayingNotes(cts);
            Main.SetGameStatus("Finished");
            Main.SetPlayingMusicStatus("Finished");
            _GamePlayingDisplay.GameFinish();
            _GridPlayArea.GameFinish();
            ResultSave();
        }

        private void StopDisplayingNotes(CancellationTokenSource cts)
        {
            cts.Cancel();
        }

        private void ResultSave()
        {
            
        }

        // ゲーム中に常駐させる処理
        private void StartTaskKeepMovingDuringGame(CancellationToken ct)
        {
            TaskKeepMovingDuringGame = Task.Run(() =>
            {
                DisplayNotesAndCheckMissedNotes(ct);
            });
        }

        private void DisplayNotesAndCheckMissedNotes(CancellationToken ct)
        {
            while (true)
            {
                _GridPlayArea.JudgeLine.Dispatcher.Invoke(new Action(() =>
                {
                    TimeSpan now = GameTimer.Elapsed;
                    DisplayNotesAndCheckMissedNotesCore(now);
                }));

                if (ct.IsCancellationRequested)
                {
                    return;
                }
            }
        }

        private void DisplayNotesAndCheckMissedNotesCore(TimeSpan now)
        {
            foreach (Note note in Music.Notes)
            {
                if (note.AlreadyJudged())
                {
                    continue;
                }

                JudgeBadWhenNotePassedJudgeLineForAWhile(note, now);
                note.CalcNowPoint(_GamePlayingArea, now);
                DisplayNote(note);
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

        private void DisplayNote(Note note)
        {
            if (_GamePlayingArea.IsInsidePlayArea(note))
            {
                _GamePlayingDisplay.DisplayNotePlayArea(note);
            }
        }

        private void DisplayInfo()
        {
            Main.Dispatcher.Invoke(() =>
            {
                Main.SetGameStatus("Playing");
                Main.SetPlayingMusicStatus("Playing");
                SetNotesNum(Music.NotesNum.ToString());
            });
        }

        private void SetNotesNum(string notesNum)
        {
            _GridPlayArea.NotesNum.Content = notesNum;
        }

        #endregion

        public void Start()
        {
            GameFinishTimer.Start();
            PlayMusic();
            GameTimer.Start();
        }

        #region ゲーム開始時処理の詳細

        private void PlayMusic()
        {
            Music.PlayAsync();
        }

        #endregion

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

        #region ユーザー入力によるNoteのJudge、詳細

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
                _GamePlayingDisplay.DisplayNoteJudgeResult(note);
            }
        }

        #endregion
    }
}

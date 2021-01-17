using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class GamePlayingDisplay
    {
        private GridPlayArea _GridPlayArea { get; set; }

        private JudgeResultImage _JudgeResultImage { get; set; }

        private System.Timers.Timer CountdownTimer { get; set; }

        // 処理高速化のために判定結果を別に保持するための変数
        private int PerfectNum { get; set; } = 0;

        // 処理高速化のために判定結果を別に保持するための変数
        private int GoodNum { get; set; } = 0;

        // 処理高速化のために判定結果を別に保持するための変数
        private int BadNum { get; set; } = 0;

        public GamePlayingDisplay(GridPlayArea playArea, int notesNum)
        {
            _GridPlayArea = playArea;
            _JudgeResultImage = new JudgeResultImage(_GridPlayArea, GetJudgeResultDisplayCenter());
            InitDisplayingPlayAreaResult(notesNum);
        }

        private void InitDisplayingPlayAreaResult(int notesNum)
        {
            _GridPlayArea.NotesNum.Content = notesNum.ToString();
            _GridPlayArea.ResultPerfect.Content = "0";
            _GridPlayArea.ResultGood.Content = "0";
            _GridPlayArea.ResultBad.Content = "0";
        }

        private System.Windows.Point GetJudgeResultDisplayCenter()
        {
            // TODO: 設定値の外部管理化
            return new System.Windows.Point(_GridPlayArea.PlayArea.ActualWidth / 2, _GridPlayArea.PlayArea.ActualHeight - 200);
        }

        public void DisplayStartingWait(int countdownStartSecond)
        {
            DisplayCountdown(countdownStartSecond);
            StartDisplayCountdown(countdownStartSecond - 1);
            WaitToFinishCountdown();
        }

        #region private: ゲーム開始時のカウントダウン処理詳細

        private void DisplayCountdown(int second)
        {
            _GridPlayArea.Dispatcher.Invoke(() =>
            {
                if (second == 1)
                {
                    DisplayGameStatus("Ready");
                }
                else if (second == 0)
                {
                    DisplayGameStatus("Playing...");
                }
                else
                {
                    DisplayGameStatus(second.ToString());
                }
            });
        }

        private void StartDisplayCountdown(int countdownStartSecond)
        {
            int countdownSecond = countdownStartSecond;
            CountdownTimer = new System.Timers.Timer();

            CountdownTimer.Interval = 1000;  // 1[s]
            CountdownTimer.Elapsed += (s, e) =>
            {
                DisplayCountdown(countdownSecond);
                if (countdownSecond <= 0)
                {
                    CountdownTimer.Stop();
                    CountdownTimer.Enabled = false;
                }
                countdownSecond--;
            };

            CountdownTimer.Start();
        }

        private void WaitToFinishCountdown()
        {
            Task task = Task.Run(() =>
            {
                while (true)
                {
                    if (!CountdownTimer.Enabled)
                    {
                        return;
                    }
                }
            });

            Task.WaitAll(task);
        }

        #endregion

        public void DisplayGameStatus(string status)
        {
            _GridPlayArea.GameStatus.Content = status;
        }

        public void DisplayNotePlayArea(Note note)
        {
            note.DisplayPlayArea(_GridPlayArea.PlayArea.Children);
        }

        public void DisplayNoteJudgeResult(Note note)
        {
            DisplayNoteJudgeResultImage(note.JudgeResult);
            RemoveNotePlayArea(note);
            UpdateJudgeResult(note.JudgeResult);
        }

        #region private: Judge結果の画面への反映処理詳細

        private void DisplayNoteJudgeResultImage(NoteJudge.JudgeType result)
        {
            _JudgeResultImage.Show(result);
        }

        private void RemoveNotePlayArea(Note note)
        {
            note.HidePlayArea(_GridPlayArea.PlayArea.Children);
        }

        private void UpdateJudgeResult(NoteJudge.JudgeType result)
        {
            if (result == NoteJudge.JudgeType.Perfect)
            {
                PerfectNum++;
                _GridPlayArea.ResultPerfect.Content = PerfectNum.ToString();
            }
            else if (result == NoteJudge.JudgeType.Good)
            {
                GoodNum++;
                _GridPlayArea.ResultGood.Content = GoodNum.ToString();
            }
            else if (result == NoteJudge.JudgeType.Bad)
            {
                BadNum++;
                _GridPlayArea.ResultBad.Content = BadNum.ToString();
            }
        }

        #endregion

        public void GameFinish()
        {
            DisplayGameStatus("Finished");
            _GridPlayArea.ResultFinishNotesNum.Content = _GridPlayArea.NotesNum.Content;
            _GridPlayArea.ResultFinishPerfect.Content = _GridPlayArea.ResultPerfect.Content;
            _GridPlayArea.ResultFinishGood.Content = _GridPlayArea.ResultGood.Content;
            _GridPlayArea.ResultFinishBad.Content = _GridPlayArea.ResultBad.Content;
        }

        public void GameSuspend()
        {
            DisplayGameStatus("Suspending...");
        }

        public void GameRestart()
        {
            DisplayGameStatus("Playing...");
        }
    }
}

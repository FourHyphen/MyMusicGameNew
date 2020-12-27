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

        private int PerfectNum { get; set; } = 0;

        private int GoodNum { get; set; } = 0;

        private int BadNum { get; set; } = 0;

        public GamePlayingDisplay(GridPlayArea playArea)
        {
            _GridPlayArea = playArea;
            _JudgeResultImage = new JudgeResultImage(_GridPlayArea, GetJudgeResultDisplayCenter());
            InitDisplayingPlayAreaResult();
        }

        private void InitDisplayingPlayAreaResult()
        {
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

        private void DisplayCountdown(int second)
        {
            _GridPlayArea.Dispatcher.Invoke(() =>
            {
                if (second == 1)
                {
                    _GridPlayArea.GameStatus.Content = "Ready";
                }
                else if (second == 0)
                {
                    _GridPlayArea.GameStatus.Content = "Playing...";
                }
                else
                {
                    _GridPlayArea.GameStatus.Content = second.ToString();
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

        public void GameFinish()
        {
            _GridPlayArea.GameStatus.Content = "Finished";
            _GridPlayArea.ResultFinishNotesNum.Content = _GridPlayArea.NotesNum.Content;
            _GridPlayArea.ResultFinishPerfect.Content = _GridPlayArea.ResultPerfect.Content;
            _GridPlayArea.ResultFinishGood.Content = _GridPlayArea.ResultGood.Content;
            _GridPlayArea.ResultFinishBad.Content = _GridPlayArea.ResultBad.Content;
        }
    }
}

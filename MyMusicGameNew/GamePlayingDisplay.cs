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

        public GamePlayingDisplay(GridPlayArea playArea, System.Windows.Point judgeResultDisplayCenter)
        {
            _GridPlayArea = playArea;
            _JudgeResultImage = new JudgeResultImage(_GridPlayArea, judgeResultDisplayCenter);
        }

        public void DisplayStartingWait(int countdownStartSecond)
        {
            // TODO: 言語仕様の特殊なコードを隠蔽したい
            _GridPlayArea.Dispatcher.Invoke(() =>
            {
                DisplayCountdownCore(countdownStartSecond);
            });
            StartDisplayCountdownTimer(countdownStartSecond - 1);
            WaitToFinishCountdown();
        }

        private void StartDisplayCountdownTimer(int countdownStartSecond)
        {
            int countdownSecond = countdownStartSecond;
            CountdownTimer = new System.Timers.Timer();

            CountdownTimer.Interval = 1000;  // 1[s]
            CountdownTimer.Elapsed += (s, e) =>
            {
                _GridPlayArea.Dispatcher.Invoke(new Action(() =>
                {
                    DisplayCountdownCore(countdownSecond);
                    if (countdownSecond <= 0)
                    {
                        CountdownTimer.Stop();
                        CountdownTimer.Enabled = false;
                    }
                    countdownSecond--;
                }));
            };

            CountdownTimer.Start();
        }

        private void DisplayCountdownCore(int second)
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
        }

        private void WaitToFinishCountdown()
        {
            Task task = Task.Run(() =>
            {
                WaitToFinishCountdownCore();
            });

            Task.WaitAll(task);
        }

        private void WaitToFinishCountdownCore()
        {
            while (true)
            {
                if (!CountdownTimer.Enabled)
                {
                    return;
                }
            }
        }

        public void DisplayNotePlayArea(Note note)
        {
            System.Windows.Controls.Image image = note.DisplayImage;
            if (!_GridPlayArea.PlayArea.Children.Contains(image))
            {
                note.SetVisible();
                _GridPlayArea.PlayArea.Children.Add(image);
            }
        }

        public void DisplayNoteJudgeResult(Note note)
        {
            DisplayNoteJudgeResultImage(note.JudgeResult);
            RemoveNotePlayArea(note);
            UpdateJudgeResult(note.JudgeResult);
        }

        public void DisplayNoteJudgeResultImage(NoteJudge.JudgeType result)
        {
            _JudgeResultImage.Show(result);
        }

        public void RemoveNotePlayArea(Note note)
        {
            System.Windows.Controls.Image image = note.DisplayImage;
            if (_GridPlayArea.PlayArea.Children.Contains(image))
            {
                _GridPlayArea.PlayArea.Children.Remove(image);
            }
        }

        public void UpdateJudgeResult(NoteJudge.JudgeType result)
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
    }
}

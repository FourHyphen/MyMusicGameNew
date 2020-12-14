using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class GamePlayingDisplay
    {
        private GridPlayArea _GridPlayArea { get; set; }

        private JudgeResultImage _JudgeResultImage { get; set; }

        private int PerfectNum { get; set; } = 0;

        private int GoodNum { get; set; } = 0;

        private int BadNum { get; set; } = 0;

        public GamePlayingDisplay(GridPlayArea playArea, System.Windows.Point judgeResultDisplayCenter)
        {
            _GridPlayArea = playArea;
            _JudgeResultImage = new JudgeResultImage(_GridPlayArea, judgeResultDisplayCenter);
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

        public void SetTestString(Note note, int index)
        {
            // テスト用: 現在座標の表示
            if (index == 0)
            {
                string x = ((int)note.NowX).ToString().PadLeft(7);
                string y = ((int)note.NowY).ToString().PadLeft(7);
                _GridPlayArea.DisplayNotesNearestJudgeLine.Content = "(" + x + ", " + y + ")";
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class GamePlayingDisplay
    {
        private MainWindow Main { get; set; }

        private JudgeResultImage _JudgeResultImage { get; set; }

        private int PerfectNum { get; set; } = 0;

        private int GoodNum { get; set; } = 0;

        private int BadNum { get; set; } = 0;

        public GamePlayingDisplay(MainWindow main, System.Windows.Point judgeResultDisplayCenter)
        {
            Main = main;
            _JudgeResultImage = new JudgeResultImage(Main, judgeResultDisplayCenter);
        }

        public void DisplayNotePlayArea(Note note)
        {
            System.Windows.Controls.Image image = note.DisplayImage;
            if (!Main.PlayArea.Children.Contains(image))
            {
                note.SetVisible();
                Main.PlayArea.Children.Add(image);
            }
        }

        public void SetTestString(Note note, int index)
        {
            // テスト用: 現在座標の表示
            if (index == 0)
            {
                string x = ((int)note.NowX).ToString().PadLeft(7);
                string y = ((int)note.NowY).ToString().PadLeft(7);
                Main.DisplayNotesNearestJudgeLine.Content = "(" + x + ", " + y + ")";
            }
        }

        public void DisplayNoteJudgeResult(Note note)
        {
            // TODO: 結果判定時、その結果をプレイヤーが確認しやすい位置に一定時間表示する
            //  -> メッセージ：実装中：自動テスト不可、機能追加のみ
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
            if (Main.PlayArea.Children.Contains(image))
            {
                Main.PlayArea.Children.Remove(image);
            }
        }

        public void UpdateJudgeResult(NoteJudge.JudgeType result)
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

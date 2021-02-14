using System.Windows;

namespace MyMusicGameNew
{
    internal class CalcNotePointTopToBottom : CalcNotePoint
    {
        public CalcNotePointTopToBottom(int playAreaWidth,
                                        int playAreaHeight,
                                        double judgeLineXFromAreaLeft,
                                        double judgeLineYFromAreaTop,
                                        double noteSpeedRate) : base(playAreaWidth, playAreaHeight, judgeLineXFromAreaLeft, judgeLineYFromAreaTop, noteSpeedRate)
        {
        }

        public override System.Windows.Point CalcNowPoint(int lineNum, double diffMillisec, double noteSpeedXPerSec, double noteSpeedYPerSec)
        {
            double x = CalcNowX(lineNum);
            double y = CalcNowY(diffMillisec, noteSpeedYPerSec);
            return new System.Windows.Point(x, y);
        }

        private double CalcNowX(int lineNum)
        {
            int basis = (int)((double)PlayAreaWidth * 0.33333);
            return basis * lineNum;
        }

        private double CalcNowY(double diffMillisec, double noteSpeedYPerSec)
        {
            double dist = (diffMillisec / 1000.0) * (noteSpeedYPerSec * NoteSpeedRate);
            return JudgeLineYFromAreaTop - dist;
        }

        public override System.Windows.Point GetLinePoint(int lineNum)
        {
            double x = CalcNowX(lineNum);
            double y = JudgeLineYFromAreaTop;
            return new System.Windows.Point(x, y);
        }
    }
}
namespace MyMusicGameNew
{
    public class CalcNotePointRightToLeft : CalcNotePoint
    {
        public CalcNotePointRightToLeft(int playAreaWidth,
                                        int playAreaHeight,
                                        double judgeLineXFromAreaLeft,
                                        double judgeLineYFromAreaTop,
                                        double noteSpeedRate) : base(playAreaWidth, playAreaHeight, judgeLineXFromAreaLeft, judgeLineYFromAreaTop, noteSpeedRate)
        {
        }

        public override System.Windows.Point CalcNowPoint(int lineNum, double diffMillisec, double noteSpeedXPerSec, double noteSpeedYPerSec)
        {
            double x = CalcNowX(diffMillisec, noteSpeedXPerSec);
            double y = CalcNowY(lineNum);
            return new System.Windows.Point(x, y);
        }

        private double CalcNowX(double diffMillisec, double noteSpeedXPerSec)
        {
            double dist = (diffMillisec / 1000.0) * (noteSpeedXPerSec * NoteSpeedRate);
            return dist + JudgeLineXFromAreaLeft;
        }

        private double CalcNowY(int lineNum)
        {
            int basis = (int)((double)PlayAreaHeight * 0.33333);
            return basis * lineNum;
        }

        public override System.Windows.Point GetLinePoint(int lineNum)
        {
            double x = JudgeLineXFromAreaLeft;
            double y = CalcNowY(lineNum);
            return new System.Windows.Point(x, y);
        }
    }
}
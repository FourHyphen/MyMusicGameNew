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

        public override double CalcNowX(int lineNum, double diffMillisec, double noteSpeedXPerSec)
        {
            double dist = (diffMillisec / 1000.0) * (noteSpeedXPerSec * NoteSpeedRate);
            return dist + JudgeLineXFromAreaLeft;
        }

        public override double CalcNowX(int lineNum)
        {
            throw new System.Exception("到達しないはず");
        }

        public override double CalcNowY(int lineNum, double diffMillisec, double noteSpeedYPerSec)
        {
            int basis = (int)((double)PlayAreaHeight * 0.33333);
            return basis * lineNum;
        }

        public override double CalcNowY(int lineNum)
        {
            return CalcNowY(lineNum, 0.0, 0.0);
        }

        public override System.Windows.Point GetLinePoint(int lineNum)
        {
            double x = JudgeLineXFromAreaLeft;
            double y = CalcNowY(lineNum);
            return new System.Windows.Point(x, y);
        }
    }
}
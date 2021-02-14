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

        public override double CalcNowX(int lineNum, double diffMillisec, double noteSpeedXPerSec)
        {
            int basis = (int)((double)PlayAreaWidth * 0.33333);
            return basis * lineNum;
        }

        public override double CalcNowX(int lineNum)
        {
            return CalcNowX(lineNum, 0.0, 0.0);
        }

        public override double CalcNowY(int lineNum, double diffMillisec, double noteSpeedYPerSec)
        {
            double dist = (diffMillisec / 1000.0) * (noteSpeedYPerSec * NoteSpeedRate);
            return JudgeLineYFromAreaTop - dist;
        }

        public override double CalcNowY(int lineNum)
        {
            throw new System.Exception("到達しないはず");
        }

        public override System.Windows.Point GetLinePoint(int lineNum)
        {
            double x = CalcNowX(lineNum);
            double y = JudgeLineYFromAreaTop;
            return new System.Windows.Point(x, y);
        }
    }
}
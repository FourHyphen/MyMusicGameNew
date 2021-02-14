using System;

namespace MyMusicGameNew
{
    public abstract class CalcNotePoint
    {
        protected int PlayAreaWidth { get; set; }

        protected int PlayAreaHeight { get; set; }

        protected double JudgeLineXFromAreaLeft { get; set; }

        protected double JudgeLineYFromAreaTop { get; set; }

        protected double NoteSpeedRate { get; set; }

        protected CalcNotePoint(int playAreaWidth,
                                int playAreaHeight,
                                double judgeLineXFromAreaLeft,
                                double judgeLineYFromAreaTop,
                                double noteSpeedRate)
        {
            PlayAreaWidth = playAreaWidth;
            PlayAreaHeight = playAreaHeight;
            JudgeLineXFromAreaLeft = judgeLineXFromAreaLeft;
            JudgeLineYFromAreaTop = judgeLineYFromAreaTop;
            NoteSpeedRate = noteSpeedRate;
        }

        public static CalcNotePoint Create(int playAreaWidth,
                                           int playAreaHeight,
                                           double judgeLineXFromAreaLeft,
                                           double judgeLineYFromAreaTop,
                                           double noteSpeedRate,
                                           GamePlaying.NoteDirection noteDirection)
        {
            if (noteDirection == GamePlaying.NoteDirection.RightToLeft)
            {
                return new CalcNotePointRightToLeft(playAreaWidth, playAreaHeight, judgeLineXFromAreaLeft, judgeLineYFromAreaTop, noteSpeedRate);
            }
            else
            {
                return new CalcNotePointTopToBottom(playAreaWidth, playAreaHeight, judgeLineXFromAreaLeft, judgeLineYFromAreaTop, noteSpeedRate);
            }
        }

        public abstract System.Windows.Point CalcNowPoint(int lineNum, double diffMillisec, double noteSpeedXPerSec, double noteSpeedYPerSec);

        public abstract System.Windows.Point GetLinePoint(int lineNum);
    }
}
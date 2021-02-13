using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class GamePlayingArea
    {
        private int JudgeLineXFromAreaLeft { get; set; }

        private int JudgeLineYFromAreaTop { get; set; }

        private int PlayAreaWidth { get; set; }

        private int PlayAreaHeight { get; set; }

        private double NoteSpeedRate { get; }

        public GamePlayingArea(int playAreaWidth,
                               int playAreaHeight,
                               int judgeLineXFromAreaLeft,
                               int judgeLineYFromAreaTop,
                               double noteSpeedRate)
        {
            PlayAreaWidth = playAreaWidth;
            PlayAreaHeight = playAreaHeight;
            JudgeLineXFromAreaLeft = judgeLineXFromAreaLeft;
            JudgeLineYFromAreaTop = judgeLineYFromAreaTop;
            NoteSpeedRate = noteSpeedRate;
        }

        public bool IsInsidePlayArea(Note note)
        {
            double x = note.NowX;
            double y = note.NowY;
            return ((0.0 <= x && x < PlayAreaWidth) &&
                    (0.0 <= y && y < PlayAreaHeight));
        }

        public System.Windows.Point CalcNowPoint(NoteData noteData,
                                                 TimeSpan now,
                                                 double noteSpeedXPerSec,
                                                 double noteSpeedYPerSec,
                                                 GamePlaying.NoteDirection noteDirection)
        {
            double diffMillsec = Common.DiffMillisecond(noteData.JudgeOfJustTiming, now);
            double nowX = CalcNowX(noteData, diffMillsec, noteSpeedXPerSec, noteDirection);
            double nowY = CalcNowY(noteData, diffMillsec, noteSpeedYPerSec, noteDirection);
            return new System.Windows.Point(nowX, nowY);
        }

        private double CalcNowX(NoteData noteData, double diffMillisec, double noteSpeedXPerSec, GamePlaying.NoteDirection noteDirection)
        {
            if (noteDirection == GamePlaying.NoteDirection.RightToLeft)
            {
                return CalcNowXRightToLeft(diffMillisec, noteSpeedXPerSec);
            }
            else
            {
                return CalcNowXTopToBottom(noteData.XJudgeLinePosition);
            }
        }

        public double CalcNowXRightToLeft(double diffMillisec, double noteSpeedXPerSec)
        {
            double dist = (diffMillisec / 1000.0) * (noteSpeedXPerSec * NoteSpeedRate);
            return dist + JudgeLineXFromAreaLeft;
        }

        private double CalcNowXTopToBottom(int lineNum)
        {
            int basis = (int)((double)PlayAreaWidth * 0.33333);
            return basis * lineNum;
        }

        private double CalcNowY(NoteData noteData, double diffMillisec, double noteSpeedYPerSec, GamePlaying.NoteDirection noteDirection)
        {
            if (noteDirection == GamePlaying.NoteDirection.RightToLeft)
            {
                return CalcNowYRightToLeft(noteData.XJudgeLinePosition);
            }
            else
            {
                return CalcNowYTopToBottom(diffMillisec, noteSpeedYPerSec);
            }
        }

        private double CalcNowYRightToLeft(int lineNum)
        {
            int basis = (int)((double)PlayAreaHeight * 0.33333);
            return basis * lineNum;
        }

        private double CalcNowYTopToBottom(double diffMillisec, double noteSpeedYPerSec)
        {
            double dist = (diffMillisec / 1000.0) * (noteSpeedYPerSec * NoteSpeedRate);
            return JudgeLineYFromAreaTop - dist;
        }

        public double GetLinePointXTopToBottom(int lineNum)
        {
            return CalcNowXTopToBottom(lineNum);
        }

        public double GetLinePointYTopToBottom()
        {
            return JudgeLineYFromAreaTop;
        }

        public double GetLinePointXRightToLeft()
        {
            return JudgeLineXFromAreaLeft;
        }

        public double GetLinePointYRightToLeft(int lineNum)
        {
            return CalcNowYRightToLeft(lineNum);
        }

        public int ConvertLine(System.Windows.Point mouseClicked, GamePlaying.NoteDirection noteDirection)
        {
            if (noteDirection == GamePlaying.NoteDirection.RightToLeft)
            {
                return ConvertYLine(mouseClicked.Y);
            }
            else
            {
                return ConvertXLine(mouseClicked.X);
            }
        }

        public int ConvertYLine(double y)
        {
            // TODO: 汚い...
            double halfHeight = PlayAreaHeight / 2;
            if (y <= halfHeight)
            {
                return 1;
            }
            else if (y > halfHeight)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        public int ConvertXLine(double x)
        {
            // TODO: 汚い...
            double halfWidth = PlayAreaWidth / 2;
            if (x <= halfWidth)
            {
                return 1;
            }
            else if (x > halfWidth)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        public int ConvertLine(Keys.EnableKeys key)
        {
            if (key == Keys.EnableKeys.JudgeLine1)
            {
                return 1;
            }
            else if (key == Keys.EnableKeys.JudgeLine2)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
    }
}

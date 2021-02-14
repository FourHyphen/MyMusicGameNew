using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class GamePlayingArea
    {
        private int PlayAreaWidth { get; set; }

        private int PlayAreaHeight { get; set; }

        private CalcNotePoint _CalcNotePoint { get; set; }

        public GamePlayingArea(int playAreaWidth, int playAreaHeight, CalcNotePoint calcNotePoint)
        {
            PlayAreaWidth = playAreaWidth;
            PlayAreaHeight = playAreaHeight;
            _CalcNotePoint = calcNotePoint;
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
                                                 double noteSpeedYPerSec)
        {
            double diffMillsec = Common.DiffMillisecond(noteData.JudgeOfJustTiming, now);
            int lineNum = noteData.XJudgeLinePosition;
            double nowX = _CalcNotePoint.CalcNowX(lineNum, diffMillsec, noteSpeedXPerSec);
            double nowY = _CalcNotePoint.CalcNowY(lineNum, diffMillsec, noteSpeedYPerSec);
            return new System.Windows.Point(nowX, nowY);
        }

        public System.Windows.Point GetLinePoint(int lineNum)
        {
            return _CalcNotePoint.GetLinePoint(lineNum);
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

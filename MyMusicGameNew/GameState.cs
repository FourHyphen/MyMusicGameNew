using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class GameState
    {
        protected GridPlayArea _GridPlayArea { get; }

        public GameState(GridPlayArea playArea)
        {
            _GridPlayArea = playArea;
        }
    }
}

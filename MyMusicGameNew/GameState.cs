using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    public class GameState
    {
        protected MainWindow Main { get; }

        public GameState(MainWindow main)
        {
            Main = main;
        }

        protected void SetGameStatus(string status)
        {
            Main.GameStatus.Content = status;
        }

        protected void SetPlayingMusicStatus(string status)
        {
            Main.PlayingMusicStatus.Content = status;
        }

    }
}

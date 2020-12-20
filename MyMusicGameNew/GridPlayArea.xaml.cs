using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyMusicGameNew
{
    public partial class GridPlayArea : UserControl
    {
        private MainWindow Main { get; set; }

        private bool IsTest { get; set; } = false;

        private GamePlaying GamePlay { get; set; } = null;

        public GridPlayArea()
        {
            InitializeComponent();
        }

        public void Init(MainWindow main, bool isTest)
        {
            Main = main;
            IsTest = isTest;
        }

        public void GameStart(MainWindow main, string musicName)
        {
            GamePlayingArea area = new GamePlayingArea((int)PlayArea.ActualWidth, (int)PlayArea.ActualHeight);
            GamePlay = new GamePlaying(main, this, area, musicName, IsTest);
            GameStartCore();
        }

        private async void GameStartCore()
        {
            await Task.Run(() =>
            {
                GamePlay.Starting();
                GamePlay.Start();
            });
        }

        private void PlayAreaMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DoGamePlaying())
            {
                Point p = e.GetPosition(this);
                Judge(p);
            }
        }

        public bool DoGamePlaying()
        {
            return (GamePlay != null);
        }

        private void Judge(System.Windows.Point p)
        {
            GamePlay.Judge(p);
        }

        public void Judge(KeyEventArgs e)
        {
            if (DoGamePlaying())
            {
                Keys.EnableKeys key = Keys.ToEnableKeys(e.Key, e.KeyboardDevice);
                Judge(key);
            }
        }

        private void Judge(Keys.EnableKeys key)
        {
            GamePlay.Judge(key);
        }

        public void GameFinish()
        {
            Main.GameFinish();
        }
    }
}

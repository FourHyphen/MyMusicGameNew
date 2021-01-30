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

        public void GameStart(Music music, double noteSpeedRate)
        {
            GamePlay = new GamePlaying(this, music, noteSpeedRate, IsTest);
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
            Point p = e.GetPosition(this);
            Judge(p);
        }

        public void ProcessKeyDown(Keys.EnableKeys key)
        {
            if (IsKeyDownForJudge(key))
            {
                Judge(key);
            }
            else if (IsKeyDownForSuspend(key))
            {
                SuspendGame();
            }
            else if (IsKeyDownForRestart(key))
            {
                RestartGame();
            }
        }

        #region private: キー入力種別判断の詳細

        private bool IsKeyDownForJudge(Keys.EnableKeys key)
        {
            return (key == Keys.EnableKeys.JudgeLine1 || key == Keys.EnableKeys.JudgeLine2);
        }

        private bool IsKeyDownForSuspend(Keys.EnableKeys key)
        {
            return (key == Keys.EnableKeys.Suspend);
        }

        private bool IsKeyDownForRestart(Keys.EnableKeys key)
        {
            return (key == Keys.EnableKeys.Restart);
        }

        #endregion

        #region private Judge詳細

        private void Judge(System.Windows.Point mousePoint)
        {
            if (DoGamePlaying())
            {
                GamePlay.Judge(mousePoint);
            }
        }

        private void Judge(Keys.EnableKeys key)
        {
            if (DoGamePlaying())
            {
                GamePlay.Judge(key);
            }
        }

        #endregion

        public bool DoGamePlaying()
        {
            if (GamePlay == null)
            {
                return false;
            }
            return (GamePlay.DoPlaying());
        }

        #region ゲーム中断/再開処理

        private void SuspendGame()
        {
            ShowSuspend();
            GamePlay.Suspend();
        }

        private void ShowSuspend()
        {
            Suspend.Visibility = Visibility.Visible;
        }

        private void RestartGame()
        {
            HideSuspend();
            GamePlay.Restart();
        }

        private void HideSuspend()
        {
            Suspend.Visibility = Visibility.Hidden;
        }

        private void RestartClicked(object sender, RoutedEventArgs e)
        {
            RestartGame();
        }

        #endregion

        #region ゲーム終了時処理

        public void GameFinish()
        {
            PlayResult.Visibility = Visibility.Visible;
        }

        private void PlayResultOKClicked(object sender, RoutedEventArgs e)
        {
            DisplayMusicSelect();
        }

        private void DisplayMusicSelect()
        {
            Main.GameFinish();
            PlayResult.Visibility = Visibility.Hidden;
        }

        #endregion
    }
}

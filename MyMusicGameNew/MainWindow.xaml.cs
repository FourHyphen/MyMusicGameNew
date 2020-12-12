﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public partial class MainWindow : Window
    {
        private bool IsTest { get; set; }

        private GameMusicSelect MusicSelect { get; set; } = null;

        private GamePlaying GamePlay { get; set; } = null;

        public MainWindow()
        {
            InitializeComponent();
            SetEnvironmentCurrentDirectory(Environment.CurrentDirectory + "../../../");  // F5開始を想定
            MusicSelect = new GameMusicSelect(this);
            MusicSelect.Init();
        }

        private void SetEnvironmentCurrentDirectory(string environmentDirPath)
        {
            // TODO: 配布を考えると、ここと同階層にGameDataディレクトリがある場合/ない場合で分岐すべき
            Environment.CurrentDirectory = environmentDirPath;
            IsTest = (Environment.CurrentDirectory.Contains("TestMyMusicGameNew"));
        }

        private void GameStartButtonClick(object sender, RoutedEventArgs e)
        {
            GameStart();
        }

        private void GameStart()
        {
            if (MusicSelect.MusicSelected())
            {
                string musicName = MusicSelect.GetSelectedMusicName();
                GameStartCore(musicName);
            }
        }

        private void GameStartCore(string musicName)
        {
            GamePlayingArea area = new GamePlayingArea((int)PlayArea.ActualWidth, (int)PlayArea.ActualHeight);
            GamePlay = new GamePlaying(this, area, musicName, IsTest);
            GamePlay.Start();
        }

        private void PlayAreaMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DoGamePlaying())
            {
                Point p = e.GetPosition(this);
                Judge(p);
            }
        }

        private bool DoGamePlaying()
        {
            return (GamePlay != null);
        }

        private void Judge(System.Windows.Point p)
        {
            GamePlay.Judge(p);
        }

        private void MainWindowKeyDown(object sender, KeyEventArgs e)
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
    }
}

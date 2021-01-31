using System;
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

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }


        private void Init()
        {
            SetEnvironmentCurrentDirectory(Environment.CurrentDirectory + "../../../");  // F5開始を想定
            ShowMusicSelect();
        }

        private void SetEnvironmentCurrentDirectory(string environmentDirPath)
        {
            // TODO: 配布を考えると、ここと同階層にGameDataディレクトリがある場合/ない場合で分岐すべき
            Environment.CurrentDirectory = environmentDirPath;
            IsTest = (Environment.CurrentDirectory.Contains("TestMyMusicGameNew"));
        }

        private void ShowMusicSelect()
        {
            MusicSelect.Init(this);
            MusicSelect.Visibility = Visibility.Visible;
            PlayArea.Visibility = Visibility.Hidden;
        }

        #region キー入力処理

        private void MainWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (DoGamePlaying())
            {
                Keys.EnableKeys key = Keys.ToEnableKeys(e.Key, e.KeyboardDevice);
                PlayArea.ProcessKeyDown(key);
            }
        }

        #endregion

        #region 他画面との連携

        private bool DoGamePlaying()
        {
            return PlayArea.DoGamePlaying();
        }

        public void GameStart(Music music, double noteSpeedRate)
        {
            ShowPlayArea();
            PlayArea.GameStart(music, noteSpeedRate);
        }

        private void ShowPlayArea()
        {
            PlayArea.Init(this, IsTest);
            MusicSelect.Visibility = Visibility.Hidden;
            PlayArea.Visibility = Visibility.Visible;
        }

        public void GameFinish()
        {
            ShowMusicSelect();
        }

        #endregion
    }
}

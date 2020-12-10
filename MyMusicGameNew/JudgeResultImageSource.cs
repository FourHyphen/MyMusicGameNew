using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyMusicGameNew
{
    public class JudgeResultImageSource
    {
        public Image DisplayImage { get; private set; }

        private BitmapSource Source { get; set; }

        private string ResultName { get; set; }

        private System.Timers.Timer DisplayTimer { get; set; }

        public JudgeResultImageSource(MainWindow main, string resultName, string displayImagePath, System.Windows.Point center)
        {
            ResultName = resultName;
            Init(main, displayImagePath, center);
        }

        private void Init(MainWindow main, string displayImagePath, System.Windows.Point center)
        {
            Source = Common.GetImage(displayImagePath);
            CreateDisplayImage(Source, center);
            // TODO: 表示時間の外部管理化
            InitDisplayTimer(main, 500);
        }

        ~JudgeResultImageSource()
        {
            if (DisplayTimer != null)
            {
                DisplayTimer.Stop();
                DisplayTimer.Dispose();
                DisplayTimer = null;
            }
        }

        private void CreateDisplayImage(BitmapSource source, System.Windows.Point center)
        {
            System.Windows.Point imagePosition = GetImagePosition(source.Width, source.Height, center);

            DisplayImage = new Image();
            DisplayImage.Source = source.Clone();
            DisplayImage.Stretch = Stretch.None;
            DisplayImage.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            DisplayImage.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DisplayImage.Name = ResultName;
            DisplayImage.RenderTransform = GetNotesTransform(imagePosition.X, imagePosition.Y);
            DisplayImage.Visibility = System.Windows.Visibility.Visible;
        }

        private System.Windows.Point GetImagePosition(double imageWidth, double imageHeight, System.Windows.Point center)
        {
            // 表示したい中心位置に合わせて、表示する画像の(left, top)位置を算出する
            double left = center.X - (imageWidth / 2.0);
            double top = center.Y - (imageHeight / 2.0);
            return new System.Windows.Point(left, top);
        }

        private Transform GetNotesTransform(double x, double y)
        {
            var transform = new TransformGroup();
            transform.Children.Add(new TranslateTransform(x, y));

            return transform;
        }

        private void InitDisplayTimer(MainWindow main, int displayTimeMilliseconds)
        {
            DisplayTimer = new System.Timers.Timer();
            DisplayTimer.AutoReset = false;  // 1回だけタイマー処理を実行
            DisplayTimer.Interval = displayTimeMilliseconds;
            DisplayTimer.Elapsed += (s, e) =>
            {
                main.Dispatcher.Invoke(new Action(() =>
                {
                    RemoveShowingJudgeResultImage(main);
                }));
            };
        }

        public void RemoveShowingJudgeResultImage(MainWindow main)
        {
            if (main.PlayArea.Children.Contains(DisplayImage))
            {
                main.PlayArea.Children.Remove(DisplayImage);
            }
        }

        public void Show(MainWindow main)
        {
            // TODO: 表示時間の外部管理
            ShowPeriod(main);
        }

        private void ShowPeriod(MainWindow main)
        {
            // すでに表示してるなら表示処理を中断
            if (AlreadyDisplaying(main))
            {
                InterruptShowing(main);
            }

            // 表示＆一定時間後に非表示にする
            ShowAndSetHideTimer(main);
        }

        private bool AlreadyDisplaying(MainWindow main)
        {
            return (main.PlayArea.Children.Contains(DisplayImage));
        }

        private void InterruptShowing(MainWindow main)
        {
            // TimerはStopするだけで経過時刻がリセットされる
            // 再StartにあたってStop時点での経過時刻は引き継がれず、新規に時刻カウントし始める
            DisplayTimer.Stop();
            RemoveShowingJudgeResultImage(main);
        }

        private void ShowAndSetHideTimer(MainWindow main)
        {
            main.PlayArea.Children.Add(DisplayImage);
            DisplayTimer.Start();
        }
    }
}

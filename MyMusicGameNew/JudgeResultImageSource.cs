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

        public JudgeResultImageSource(string resultName, string displayImagePath, System.Windows.Point center)
        {
            ResultName = resultName;
            Init(displayImagePath, center);
        }

        private void Init(string displayImagePath, System.Windows.Point center)
        {
            Source = Common.GetImage(displayImagePath);
            CreateDisplayImage(Source, center);
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
            ShowPeriod(main, 500);
        }

        private void ShowPeriod(MainWindow main, int displayTimeMilliseconds)
        {
            // すでに表示してるなら表示処理を中断
            if (AlreadyDisplaying())
            {
                InterruptShowing(main);
            }

            // 表示
            main.PlayArea.Children.Add(DisplayImage);

            // 一定時間後に非表示にする
            SetDisplayTimer(main, displayTimeMilliseconds);
        }

        private bool AlreadyDisplaying()
        {
            return (DisplayTimer != null);
        }

        private void InterruptShowing(MainWindow main)
        {
            DisplayTimer.Stop();
            DisplayTimer.Dispose();
            Remove(main);
            DisplayTimer = null;
        }

        private void Remove(MainWindow main)
        {
            if (main.PlayArea.Children.Contains(DisplayImage))
            {
                main.PlayArea.Children.Remove(DisplayImage);
            }
        }

        private void SetDisplayTimer(MainWindow main, int displayTimeMilliseconds)
        {
            DisplayTimer = new System.Timers.Timer();
            DisplayTimer.AutoReset = false;  // 1回だけタイマー処理を実行
            DisplayTimer.Interval = displayTimeMilliseconds;
            DisplayTimer.Elapsed += (s, e) =>
            {
                main.Dispatcher.Invoke(new Action(() =>
                {
                    Remove(main);
                }));
            };
            DisplayTimer.Start();
        }
    }
}

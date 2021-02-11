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
    public class NoteImage
    {
        private Image DisplayImage { get; set; }

        private BitmapSource NotesBitmap { get; set; }

        private double XDisplayAdjust { get; set; }

        private double YDisplayAdjust { get; set; }

        public NoteImage(int index, string noteImagePath = "./GameData/NoteImage/note.png")
        {
            Init(noteImagePath, index);
        }

        private void Init(string noteImagePath, int index)
        {
            InitBitmap(noteImagePath);
            CreateDisplayImage(index);
        }

        private void InitBitmap(string noteImagePath)
        {
            NotesBitmap = Common.GetImage(noteImagePath);
            XDisplayAdjust = NotesBitmap.Width / 2.0;
            YDisplayAdjust = NotesBitmap.Height / 2.0;
        }

        private void CreateDisplayImage(int index)
        {
            DisplayImage = new Image();
            DisplayImage.Source = NotesBitmap.Clone();
            DisplayImage.Stretch = Stretch.None;
            DisplayImage.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            DisplayImage.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            DisplayImage.Name = "Note" + index.ToString();
        }

        public void SetNowCoordinate(System.Windows.Point nowPoint)
        {
            DisplayImage.RenderTransform = GetNotesTransform(nowPoint.X, nowPoint.Y);
        }

        private Transform GetNotesTransform(double x, double y)
        {
            var transform = new TransformGroup();
            double imageLeft = x - XDisplayAdjust;
            double imageTop = y - YDisplayAdjust;
            transform.Children.Add(new TranslateTransform(imageLeft, imageTop));

            return transform;
        }

        public void DisplayPlayArea(System.Windows.Controls.UIElementCollection collection)
        {
            if (!collection.Contains(DisplayImage))
            {
                SetVisible();
                collection.Add(DisplayImage);
            }
        }

        private void SetVisible()
        {
            DisplayImage.Visibility = System.Windows.Visibility.Visible;
        }

        public void HidePlayArea(System.Windows.Controls.UIElementCollection collection)
        {
            if (collection.Contains(DisplayImage))
            {
                collection.Remove(DisplayImage);
            }
        }
    }
}

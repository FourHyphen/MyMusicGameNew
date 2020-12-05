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
        public Image DisplayImage { get; private set; }

        private BitmapSource NotesBitmap { get; set; }

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
            transform.Children.Add(new TranslateTransform(x, y));

            return transform;
        }

        public void SetVisible()
        {
            DisplayImage.Visibility = System.Windows.Visibility.Visible;
        }
    }
}

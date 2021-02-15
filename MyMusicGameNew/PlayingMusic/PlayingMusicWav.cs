using System;
using System.IO;
using System.Windows.Controls;

namespace MyMusicGameNew
{
    public class PlayingMusicWav : PlayingMusic
    {
        private GridPlayArea PlayArea { get; }

        public PlayingMusicWav(string dataPath, GridPlayArea playArea) : base(dataPath)
        {
            PlayArea = playArea;
            Init();
        }

        private void Init()
        {
            CheckExistDataFile();

            PlayArea.MusicMedia.LoadedBehavior = MediaState.Manual;
            PlayArea.MusicMedia.Source = new Uri(DataPath);

            // 最初の処理に時間かかるので前もって準備完了させる
            PlayArea.MusicMedia.Play();
            PlayArea.MusicMedia.Stop();
        }

        private void CheckExistDataFile()
        {
            if (!System.IO.File.Exists(DataPath))
            {
                throw new FileNotFoundException(DataPath);
            }
        }

        public override void PlayAsync()
        {
            PlayArea.Dispatcher.Invoke(new Action(() =>
            {
                PlayArea.MusicMedia.Play();
            }));
        }

        public override void Stop()
        {
            PlayArea.Dispatcher.Invoke(new Action(() =>
            {
                PlayArea.MusicMedia.Pause();
            }));
        }

        public override void Restart()
        {
            PlayAsync();
        }
    }
}

using System;
using System.IO;

namespace MyMusicGameNew
{
    public class NoteSEWav : NoteSE
    {
        private System.Media.SoundPlayer Player { get; }

        public NoteSEWav()
        {
            string seDataFilePath = Common.GetFilePathOfDependentEnvironment("/GameData/NoteSE/type0001.wav");
            CheckExistFile(seDataFilePath);
            Player = new System.Media.SoundPlayer(seDataFilePath);

            // 初期化に時間かかるので前もって完了させておく
            Player.Play();
            Player.Stop();
        }

        private void CheckExistFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }
        }

        public void Sound()
        {
            Player.Stop();    // 重複して鳴らさないようにする(既存音ゲーの挙動と合わせる)
            Player.Play();
        }
    }
}
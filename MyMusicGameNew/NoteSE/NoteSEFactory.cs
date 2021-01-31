using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusicGameNew
{
    class NoteSEFactory
    {
        public NoteSE Create(bool isTest)
        {
            if (isTest)
            {
                // メッセージ：実装してテスト側の動作確認
                return new NoteSEFake();
            }
            else
            {
                return new NoteSEWav();
            }
        }
    }
}

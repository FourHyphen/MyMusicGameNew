namespace MyMusicGameNew
{
    class NoteSEFactory
    {
        public NoteSE Create(bool isTest)
        {
            if (isTest)
            {
                return new NoteSEFake();
            }
            else
            {
                return new NoteSEWav();
            }
        }
    }
}
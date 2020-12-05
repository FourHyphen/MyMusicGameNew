using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyMusicGameNew
{
    public class MusicFactory
    {
        public Music Create(string musicName, int playAreaWidth, int playAreaHeight, bool isTest=false)
        {
            MusicInfo info = GetMusicInfo(musicName);
            List<Note> notes = GetNotes(info.Note, playAreaWidth, playAreaHeight);
            PlayingMusic play = GetPlayingMusic(info.MusicData, isTest);
            return new Music(musicName, info.TimeSecond, notes, play);
        }

        private MusicInfo GetMusicInfo(string musicName)
        {
            string jsonPath = Common.GetFilePathOfDependentEnvironment("/GameData/MusicInfo/" + musicName + ".json");
            string jsonStr = Common.ReadFile(jsonPath);
            return JsonConvert.DeserializeObject<MusicInfo>(jsonStr);
        }

        private List<Note> GetNotes(string notePath, int playAreaWidth, int playAreaHeight)
        {
            List<NoteData> noteData = GetNoteData(notePath);
            List<Note> noteList = new List<Note>();
            GamePlayingArea area = new GamePlayingArea(playAreaWidth, playAreaHeight);
            foreach (NoteData nd in noteData)
            {
                Note n = new Note(nd, area);
                noteList.Add(n);
            }

            return noteList;
        }

        private List<NoteData> GetNoteData(string notePath)
        {
            string jsonPath = Common.GetFilePathOfDependentEnvironment(notePath);
            string jsonStr = Common.ReadFile(jsonPath);
            return JsonConvert.DeserializeObject<List<NoteData>>(jsonStr);
        }

        private PlayingMusic GetPlayingMusic(string musicDataPath, bool isTest)
        {
            return PlayingMusicFactory.Create(musicDataPath, isTest);
        }
    }
}

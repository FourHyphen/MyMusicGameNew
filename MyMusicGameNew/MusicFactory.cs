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
        public Music Create(string musicName, bool isTest=false)
        {
            MusicInfo info = GetMusicInfo(musicName);
            List<Note> notes = GetNotes(info.Note);
            return new Music(musicName, info, notes);
        }

        private MusicInfo GetMusicInfo(string musicName)
        {
            string jsonPath = Common.GetFilePathOfDependentEnvironment("/GameData/MusicInfo/" + musicName + ".json");
            string jsonStr = Common.ReadFile(jsonPath);
            return JsonConvert.DeserializeObject<MusicInfo>(jsonStr);
        }

        private List<Note> GetNotes(string notePath)
        {
            List<NoteData> noteData = GetNoteData(notePath);
            List<Note> noteList = new List<Note>();
            foreach (NoteData nd in noteData)
            {
                Note n = new Note(nd);
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
    }
}

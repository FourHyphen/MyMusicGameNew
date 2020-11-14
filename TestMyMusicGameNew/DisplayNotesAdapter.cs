using RM.Friendly.WPFStandardControls;
using System;
using System.Windows;

namespace TestMyMusicGameNew
{
    public class DisplayNotesAdapter
    {
        private int MaxDisplayNotesNum = 200;  // 1画面に200個も表示しないだろう
        private string NoteDisplayName = "";

        public DisplayNotesAdapter(string noteDisplayName)
        {
            NoteDisplayName = noteDisplayName;
        }

        public int GetDisplayNum(IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree)
        {
            int notesNum = 0;
            for (int i = 0; i < MaxDisplayNotesNum; i++)
            {
                if (ExistNote(logicalTree, i))
                {
                    notesNum++;
                }
                else
                {
                    break;
                }
            }

            return notesNum;
        }

        private bool ExistNote(IWPFDependencyObjectCollection<System.Windows.DependencyObject> logicalTree, int index)
        {
            string noteUIName = NoteDisplayName + index.ToString();
            var note = logicalTree.ByType<System.Windows.Controls.Image>().ByName(noteUIName);
            return (note.Count != 0);
        }
    }
}
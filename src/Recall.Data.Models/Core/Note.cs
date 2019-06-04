namespace Recall.Data.Models.Core
{
    using Recall.Data.Models.Enums;
    using System.Collections.Generic;

    public class Note
    {
        public Note()
        {
            this.ChildNotes = new HashSet<Note>();
            this.Formatting = NoteFormatting.None;
            this.Type = NoteType.Note;
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public int? SeekTo { get; set; }

        public NoteFormatting Formatting { get; set; }

        public NoteType Type { get; set; }

        public int Level { get; set; }

        public int Order { get; set; }

        public int BorderThickness { get; set; }

        public string BorderColor { get; set; }

        public string BackgroundColor { get; set; }

        public string TextColor { get; set; }

        public int? ParentNoteId { get; set; }
        public Note ParentNote { get; set; }

        public int VideoId { get; set; }
        public Video Video { get; set; }

        public ICollection<Note> ChildNotes { get; set; }
    }
}

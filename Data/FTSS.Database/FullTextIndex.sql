CREATE FULLTEXT INDEX
    ON [dbo].[Notes]
        (NoteText)
    KEY INDEX [UI_Notes_Text]
    ON [NotesData]
    WITH CHANGE_TRACKING AUTO

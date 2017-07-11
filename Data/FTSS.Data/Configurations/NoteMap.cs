namespace FTSS.Data.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using FTSS.Data.Models;

    public class NoteMap : EntityTypeConfiguration<Note>
    {
        public NoteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
        }
    }
}

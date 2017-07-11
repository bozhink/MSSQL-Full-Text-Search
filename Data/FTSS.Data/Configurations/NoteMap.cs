namespace FTSS.Data.Configurations
{
    using System.Data.Entity.ModelConfiguration;
    using FTSS.Data.Models;

    public class NoteMap : EntityTypeConfiguration<Note>
    {
        public NoteMap()
        {
            this.HasKey(t => t.Id);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NoteText)
                .IsRequired();
        }
    }
}

namespace FTSS.Data
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure.Interception;
    using FTSS.Data.Configurations;
    using FTSS.Data.Interceptors;
    using FTSS.Data.Models;

    public class FtssContext : DbContext
    {
        public FtssContext()
        {
            DbInterception.Add(new FtsInterceptor());
        }

        public FtssContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            DbInterception.Add(new FtsInterceptor());
        }

        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new NoteMap());
        }
    }
}

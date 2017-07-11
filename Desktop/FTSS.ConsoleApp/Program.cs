namespace FTSS.ConsoleApp
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using FTSS.Data;
    using FTSS.Data.Interceptors;
    using FTSS.Data.Models;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var data = DoSearch("content").Result;

            foreach (var item in data)
            {
                Console.WriteLine(item.NoteText);
            }
        }

        public static async Task<Note[]> DoSearch(string text)
        {
            var s = FtsInterceptor.Fts(text);

            using (var db = new FtssContext("Connection string"))
            {
                var query = db.Notes.Where(n => n.NoteText.Contains(s));

                var data = await query.Take(10).ToArrayAsync();
                return data;
            }
        }
    }
}

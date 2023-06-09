using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class SongDbContext : DbContext
    {
        public DbSet<OriginalSong> OriginalSongs { get; set; }
        public DbSet<UpdatedSong> UpdatedSongs { get; set; }

        public SongDbContext() : base("SongDBConnectionString")
        {
            // Uncomment the line below if you want Entity Framework to recreate the database if the model changes
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SongDbContext>());
        }

        public static void CreateNewDataBase(List<Song> songs)
        {
            using (SongDbContext context = new SongDbContext())
            {
                foreach (Song s in songs)
                {
                    // Create a new OriginalSong instance
                    OriginalSong originalSong = new OriginalSong
                    {
                        Title = s.Title,
                        Artists = s.Artists,
                        AlbumName = s.AlbumName,
                        Year = s.Year,
                        AlbumCover = s.AlbumCover,
                        Genre = s.Genre,
                        Lyrics = s.Lyrics,
                        Lyricist = s.Lyricist
                    };

                    // Add the new original song to the DbSet
                    context.OriginalSongs.Add(originalSong);
                }

                // Save changes to the database
                context.SaveChanges();
            }
        }
    }
}

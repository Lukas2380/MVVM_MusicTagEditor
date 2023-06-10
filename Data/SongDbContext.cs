using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Data
{
    public class SongDbContext : DbContext
    {
        public DbSet<OriginalSong> OriginalSongs { get; set; }
        public DbSet<UpdatedSong> UpdatedSongs { get; set; }

        public static List<Song> EditedSongs { get; set; }

        public SongDbContext() : base("SongDBConnectionString")
        {
            // Uncomment the line below if you want Entity Framework to recreate the database if the model changes
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SongDbContext>());
        }

        public static void CreateNewDataBase(List<Song> songs)
        {
            EditedSongs = new List<Song>();
            using (SongDbContext context = new SongDbContext())
            {
                // Clear the OriginalSongs table
                context.OriginalSongs.RemoveRange(context.OriginalSongs);

                // Clear the UpdatedSongs table
                context.UpdatedSongs.RemoveRange(context.UpdatedSongs);

                foreach (Song s in songs)
                {
                    // Create a new OriginalSong instance
                    OriginalSong originalSong = new OriginalSong
                    {
                        Title = s.Title,
                        Artists = s.Artists,
                        AlbumName = s.AlbumName,
                        Year = s.Year,
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

        public static void SaveChangedSongs()
        {
            using (SongDbContext context = new SongDbContext())
            {
                foreach (Song s in EditedSongs)
                {
                    // Create a new UpdatedSong instance
                    UpdatedSong updatedSong = new UpdatedSong
                    {
                        Title = s.Title,
                        Artists = s.Artists,
                        AlbumName = s.AlbumName,
                        Year = s.Year,
                        Genre = s.Genre,
                        Lyrics = s.Lyrics,
                        Lyricist = s.Lyricist
                    };

                    // Add the new original song to the DbSet
                    context.UpdatedSongs.Add(updatedSong);
                }

                // Save changes to the database
                context.SaveChanges();
                EditedSongs.Clear();
            }
        }
    }
}

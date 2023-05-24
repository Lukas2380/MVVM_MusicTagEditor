using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class SongsTable
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength (50)]
        public string SongName { get; set; }

        [Required]
        [StringLength (200)]
        public string Artists { get; set; }

        [Required]
        [StringLength (50)]
        public string AlbumName { get; set; }

        [Required]
        public int? Year { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Lyrics { get; set; }
    }
}

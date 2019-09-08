using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CloudPlayer.Models
{
    public class Track
    {
        [PrimaryKey, AutoIncrement]
        [Column("ID")]
        public  int? ID { get; set; }
        public string Title { get; set; }
        public int Artist_ID { get; set; }
        public int Album_ID { get; set; }
        public int Year { get; set; }       
        public int TrackNumber { get; set; }
        public int DiscNumber { get; set; }
        public string FileName { get; set; }
        public string OneDrive_ID { get; set; }
        public DateTimeOffset? LastUpdate { get; set; }
        public bool KeepOffline { get; set; }
        public Track()
        {
            
        }

        public async Task<Album> GetAlbum()
        {
            List<Album> albums = await App.Library.GetAlbum(Album_ID);
            if (albums.Count > 0)
                return albums[0];
            else
                throw new Exception("Album not found");
        }

        public async Task<Artist> GetArtist()
        {
            List<Artist> artists = await App.Library.GetArtist(Artist_ID);
            if (artists.Count > 0)
                return artists[0];
            else
                throw new Exception("Artist not found");
        }
    }
}


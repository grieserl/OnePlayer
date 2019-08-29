using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudPlayer.Models
{
    public class Album
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Title { get; set; }
        public int AlbumArtist_ID { get; set; }
        public bool KeepOffline { get; set; }

    }
}

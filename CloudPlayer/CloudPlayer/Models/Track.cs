using System;
using System.Collections.Generic;
using System.Text;
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
        public string FileName { get; set; }
        public string OneDrive_ID { get; set; }

        public DateTimeOffset? lastUpdate { get; set; }

        public bool KeepOffline { get; set; }
        public Track()
        {
            
        }
    }
}


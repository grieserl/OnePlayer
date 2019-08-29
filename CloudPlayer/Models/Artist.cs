using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudPlayer.Models
{
    public class Artist
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public bool KeepOffline { get; set; }


    }
}

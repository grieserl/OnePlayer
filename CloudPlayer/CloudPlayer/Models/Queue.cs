using System;
using System.Collections.Generic;
using System.Text;

namespace CloudPlayer.Models
{
    public class Queue
    {
        public int? ID { get; set; }
        public bool NowPlaying { get; set; }   
        public int Position { get; set; }
    }
}

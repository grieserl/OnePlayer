using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlayer.Models
{
    public class QueueItem
    {
        public int? track_ID { get; set; }
        public string AlbumArtPath { get; set; }
        public bool NowPlaying { get; set; }
        public int Position { get; set; }

        public async Task<Track> GetTrack()
        {
            List<Track> tracks = await App.Library.GetTrack(track_ID);
            
                return tracks[0];
        }
    }

    
}

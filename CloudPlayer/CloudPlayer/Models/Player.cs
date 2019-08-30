using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CloudPlayer.Models
{
    public class Player
    {

        public List<Queue> Queue { get; set; }
        public interface PlayMusic
        {
            Task<bool> Play(string filePath);
            Task<bool> SetVolume(float left, float right);
            int Stop();
            int Pause();
            void Initialize();
        }
        public async Task<bool> Play(Track track)
        {
            
            string url = await App.OneDrive.GetTrackURL(track.OneDrive_ID);
            return await DependencyService.Get<PlayMusic>().Play(url);
        }

        public void Initialize()
        {
            DependencyService.Get<PlayMusic>().Initialize();
        }

        public async Task LoadQueue()
        {
            Queue = await App.Library.GetQueue();
        }

        public async Task SetQueue(List<Track> tracks, Track nowPlaying = null)
        {            
            await App.Library.ClearQueue();
            foreach(Track track in tracks)
            {
                Queue queue = new Queue();
                if (track == nowPlaying)
                {
                    queue.ID = track.ID;
                    queue.Position = 0;
                    queue.NowPlaying = true;
                }
                else
                {
                    queue.ID = track.ID;
                    queue.Position = 0;
                    queue.NowPlaying = false;
                }
                await App.Library.AddToQueue(queue);
            }
            Queue = await App.Library.GetQueue();
        }
    }
}

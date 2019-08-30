using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CloudPlayer.Models
{
    public class Player
    {
        public interface PlayMusic
        {
            Task<bool> Play(string filePath);
            Task<bool> SetVolume(float left, float right);
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
    }
}

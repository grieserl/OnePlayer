using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Xamarin.Forms;
using static CloudPlayer.Models.Player;

[assembly: Dependency(typeof(CloudPlayer.UWP.PlayMusic_UWP))]
namespace CloudPlayer.UWP
{
    public class PlayMusic_UWP : PlayMusic
    {
        MediaPlayer mediaPlayer { get; set; }

        public PlayMusic_UWP()
        {
            mediaPlayer = new MediaPlayer();
        }
        public async Task<bool> Play(string filePath)
        {
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(filePath));
            mediaPlayer.Play();
            return true;

        }

        public Task<bool> SetVolume(float left, float right)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            mediaPlayer = new MediaPlayer();
        }

        public int Stop()
        {
            return Pause();
        }

        public int Pause()
        {
            mediaPlayer.Pause();
            return (int)mediaPlayer.PlaybackSession.Position.TotalMilliseconds;
        }
    }
}

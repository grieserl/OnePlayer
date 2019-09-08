using NAudio.Wave;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Xamarin.Forms;
using static CloudPlayer.Models.Player;
using static System.Net.WebRequestMethods;

[assembly: Dependency(typeof(CloudPlayer.UWP.PlayMusic_UWP))]
namespace CloudPlayer.UWP
{
    public class PlayMusic_UWP : PlayMusic
    { 
        MediaPlayer mediaPlayer { get; set; }

        public PlayMusic_UWP()
        {
            mediaPlayer = new MediaPlayer();
            mediaPlayer.MediaEnded += MediaPlayer_Completion;
        }

        public event EventHandler playbackCompleted;

        private void MediaPlayer_Completion(MediaPlayer sender, object e)
        {
            playbackCompleted.Invoke(sender, null);
        }

        /// <summary>
        ///     Play the audio file located at the filePath
        /// </summary>
        /// <param name="filePath"></param>
        public void Play(string filePath, int position)
        {            
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(filePath));
            mediaPlayer.PlaybackSession.Position = new TimeSpan(0, 0, 0, 0, position);
            mediaPlayer.Play();
        }  
        public Task<bool> SetVolume(float left, float right)
        {
            throw new NotImplementedException();
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

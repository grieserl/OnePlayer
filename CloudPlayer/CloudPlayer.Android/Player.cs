using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using CloudPlayer.Droid;
using CloudPlayer.Views;
using static CloudPlayer.Views.TestPage;
using static CloudPlayer.Models.Player;

[assembly: Dependency(typeof(PlayMusic_Android))]
namespace CloudPlayer.Droid
{
    [Activity(Label = "Player")]
    public class PlayMusic_Android : PlayMusic
    {
        MediaPlayer mediaPlayer { get; set; }
        public PlayMusic_Android()
        {
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Completion += MediaPlayer_Completion;
        }

        private void MediaPlayer_Completion (object sender, EventArgs e)
        {
            playbackCompleted.Invoke(sender, e);
        }

        public event EventHandler playbackCompleted;

        /// <summary>
        ///     Play the audio file located at the filePath
        /// </summary>
        /// <param name="filePath"></param>
        public void Play(string filePath, int position)
        {
            
            if (mediaPlayer.IsPlaying)
                mediaPlayer.Reset();
            mediaPlayer.SetDataSource(filePath);
            mediaPlayer.Prepare();
            mediaPlayer.SeekTo(position);

            mediaPlayer.Start();


        }


        public Task<bool> SetVolume(float left, float right)
        {
            mediaPlayer.SetVolume(left, right);
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            taskCompletionSource.SetResult(true);
            return taskCompletionSource.Task;
        }

        public int Stop()
        {
            mediaPlayer.Stop();
            return mediaPlayer.CurrentPosition;
        }

        public int Pause()
        {
            mediaPlayer.Pause();
            return mediaPlayer.CurrentPosition;
        }
    }
}
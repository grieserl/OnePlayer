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
        }


        public Task<bool> Play(string filePath)
        {
            if (mediaPlayer.IsPlaying)
                mediaPlayer.Reset();
            mediaPlayer.SetDataSource(filePath);
            mediaPlayer.Prepare();

            mediaPlayer.Start();
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            taskCompletionSource.SetResult(true);
            return taskCompletionSource.Task;
        }

        public Task<bool> SetVolume(float left, float right)
        {
            mediaPlayer.SetVolume(left, right);
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
            taskCompletionSource.SetResult(true);
            return taskCompletionSource.Task;
        }

        public void Initialize()
        {
            mediaPlayer = new MediaPlayer();
        }
    }
}
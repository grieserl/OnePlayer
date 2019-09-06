using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using CloudPlayer.Droid;
using Xamarin.Forms;
using static CloudPlayer.App;

[assembly: Dependency(typeof(LocalStorage_Android))]
namespace CloudPlayer.Droid
{
    [Activity(Label = "LocalStorage")]
    class LocalStorage_Android : LocalStorage
    {
        public async Task<string> GetLocalStoragePath()
        {
            
            if (!System.IO.Directory.Exists(Android.OS.Environment.ExternalStorageDirectory.ToString() + "/OnePlayer"))
                System.IO.Directory.CreateDirectory(Android.OS.Environment.ExternalStorageDirectory.ToString() + "/OnePlayer");
            return Android.OS.Environment.ExternalStorageDirectory.ToString() + "/OnePlayer";
        }
    }
}
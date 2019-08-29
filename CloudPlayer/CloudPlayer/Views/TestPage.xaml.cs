using CloudPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CloudPlayer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {
        OneDrive scanner = new OneDrive();
        public TestPage()
        {
            InitializeComponent();
        }

        private async void ScanOneDrive(object sender, EventArgs e)
        {
           
            
            await scanner.GetToken();
            await scanner.scanDriveAsync();
            App.UserSettings.LastCompletedScanDate = new DateTimeOffset(DateTime.UtcNow);
            await DisplayAlert("Finished Scanning", "", "OK");
            
        }

        private async void ClearLibrary(object sender, EventArgs e)
        {
            await App.Library.RecreateTables();
            SetupSettings(sender, e);

        }

        public async void SetupSettings (object sender, EventArgs e)
        {
            UserSettings settings = new UserSettings();
            settings.RemoteMusicPath = "Music\\";
            settings.LastCompletedScanDate = new DateTimeOffset(new DateTime(2019, 02, 28));
            await App.Library.SaveSettings(settings);
            App.UserSettings = await App.Library.GetSettings(); 


        }

        private async void PlayFirstSong(object sender, EventArgs e)
        {
            List<Track> tracks = await App.Library.GetTracks();
            string url = await scanner.GetTrackURL(tracks[0].OneDrive_ID);
            bool test = await DependencyService.Get<PlayMusic>().Play(url);
        }

        public interface PlayMusic
        {
            Task<bool> Play(string filePath);
            Task<bool> SetVolume(float left, float right);
        }
    }
}
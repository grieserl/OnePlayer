using CloudPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CloudPlayer.Models.Player;

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
            await scanner.scanDrive();
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

        private async void Pause(object sender, EventArgs e)
        {
            int test = DependencyService.Get<PlayMusic>().Pause();
        }


    }
}
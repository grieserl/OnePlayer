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
        public TestPage()
        {
            InitializeComponent();
        }

        private async void ScanOneDrive(object sender, EventArgs e)
        {
            OneDriveScanner scanner = new OneDriveScanner();
            await App.Library.ClearTracks();
            await scanner.GetToken();
            await scanner.scanDriveAsync();
            List<Track> tracks = await App.Library.GetTracks();
        }
    }
}
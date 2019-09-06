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
    public partial class NowPlayingPage : ContentPage
    {

        public NowPlayingPage()
        {
            InitializeComponent();
            
            AlbumArt.Source = ImageSource.FromFile(System.IO.Path.Combine(App.LocalStoragePath, "AlbumArt", App.Player.ArtworkPath)); 
            
        }

        public async Task Play()
        {
            await App.Player.PlayQueue();
        }

        public void SetAlbumArt()
        {
            AlbumArt.Source = ImageSource.FromFile(System.IO.Path.Combine(App.LocalStoragePath, "AlbumArt\\1.jpeg"));
        }

        public void UpdateProgress()
        {
            Progress.ProgressTo(.5, 1000, Easing.Linear);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Play();
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {

        }
    }       
}
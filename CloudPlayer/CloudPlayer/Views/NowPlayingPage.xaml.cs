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
            
        }

        protected override async void OnAppearing()
        {
            AlbumArt.Source = ImageSource.FromFile(System.IO.Path.Combine(App.LocalStoragePath, "AlbumArt", (await App.Player.GetNowPlaying()).AlbumArtPath));
        }

        public void SetAlbumArt()
        {
            AlbumArt.Source = ImageSource.FromFile(System.IO.Path.Combine(App.LocalStoragePath, "AlbumArt\\1.jpeg"));
        }

        public void UpdateProgress()
        {
            Progress.ProgressTo(.5, 1000, Easing.Linear);
        }

        private async void Previous(object sender, EventArgs e)
        {
            await App.Player.Previous();
        }

        private async void Play(object sender, EventArgs e)
        {
            await App.Player.Play();
        }

        private async void Next(object sender, EventArgs e)
        {
            await App.Player.Next();
        }
    }       
}
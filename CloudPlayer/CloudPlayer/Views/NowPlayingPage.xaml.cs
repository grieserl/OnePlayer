﻿using CloudPlayer.Models;
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

        public event EventHandler songChange;

        public NowPlayingPage()
        {
            InitializeComponent();
            App.Player.SongChanged += SongChange;
            
        }
        
        private async void SongChange(Object Sender,EventArgs e)
        {
            await SetAlbumArt();
            await SetTrackInfo();
        }

        protected override async void OnAppearing()
        {
            await SetAlbumArt();
            //await SetTrackInfo();
        }

        public async Task SetAlbumArt()
        {
            AlbumArt.Source = ImageSource.FromFile(System.IO.Path.Combine(App.LocalStoragePath, "AlbumArt", (await App.Player.GetNowPlaying()).AlbumArtPath));
        }
        public async Task SetTrackInfo()
        {
            QueueItem nowPlaying = await App.Player.GetNowPlaying();
            Track track = await nowPlaying.GetTrack();
            Title.Text = track.Title;
            Artist artist = await track.GetArtist();
            Artist.Text = artist.Name;
            Album album = await track.GetAlbum();
            Album.Text = album.Title;
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
            if (!App.Player.IsPlaying())
                await App.Player.Play();
            else
                await App.Player.Pause();
        }

        private async void Next(object sender, EventArgs e)
        {
            await App.Player.Next();
        }
    }       
}
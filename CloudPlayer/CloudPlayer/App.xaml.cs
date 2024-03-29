﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CloudPlayer.Services;
using CloudPlayer.Views;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using CloudPlayer.Models;
using System.Threading.Tasks;
using Xamarin.Forms.PlatformConfiguration;


namespace CloudPlayer
{
    public partial class App : Application
    {
        public static object ParentWindow { get; set; }
        public static GraphServiceClient GraphClient;
        public static Library Library {get;set;}
        public static Models.UserSettings UserSettings { get; set; }
        public static Player Player { get; set; }
        public static OneDrive OneDrive { get; set; }
        public static string LocalStoragePath { get; set; }

        




        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
            
        }

        protected override async void OnStart()
        {

            LocalStoragePath = await DependencyService.Get<LocalStorage>().GetLocalStoragePath();
            // Handle when your app starts
            Library = new Library();
            UserSettings = await Library.GetSettings();
            Player = new Player();
            OneDrive = new OneDrive();
            await OneDrive.GetToken();
            Player.PlayerState = Player.StateStopped;
            Player.GetNowPlaying();
            
        }

        private Task DisplayAlert(string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public interface LocalStorage
        {
            Task<string> GetLocalStoragePath();
        }
    }
}

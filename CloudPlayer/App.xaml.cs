using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CloudPlayer.Services;
using CloudPlayer.Views;
using Microsoft.Identity.Client;

namespace CloudPlayer
{
    public partial class App : Application
    {
        public static object ParentWindow { get; set; }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

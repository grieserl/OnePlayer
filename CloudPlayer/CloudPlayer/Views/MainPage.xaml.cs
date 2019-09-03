using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CloudPlayer.Models;

namespace CloudPlayer.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Browse, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Test:
                        MenuPages.Add(id, new NavigationPage(new TestPage()));
                        break;
                    case (int)MenuItemType.Artists:
                        MenuPages.Add(id, new NavigationPage(new ArtistListPage()));
                        break;
                    case (int)MenuItemType.Tracks:
                        MenuPages.Add(id, new NavigationPage(new TrackListPage()));
                        break;
                    case (int)MenuItemType.NowPlaying:
                        MenuPages.Add(id, new NavigationPage(new NowPlayingPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}
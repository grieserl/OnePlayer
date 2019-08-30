using CloudPlayer.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CloudPlayer.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrackListPage : ContentPage
    {
        public ObservableCollection<Track> Items { get; set; }

        public TrackListPage()
        {
            InitializeComponent();

            Items = new ObservableCollection<Track>();

            MyListView.ItemsSource = Items;
            SetList();
        }

        public TrackListPage(Artist artist)
        {
            InitializeComponent();

            Items = new ObservableCollection<Track>();

            MyListView.ItemsSource = Items;
            SetList(artist);
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await App.Player.Play((Track)e.Item);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        public async Task SetList()
        {
            Items = new ObservableCollection<Track>(await App.Library.GetTracks());
            MyListView.ItemsSource = Items;
        }

        public async Task SetList(Artist artist)
        {
            Items = new ObservableCollection<Track>(await App.Library.GetTracks(artist));
            MyListView.ItemsSource = Items;
        }

        private async void PlayAll(object sender, EventArgs e)
        {
            await App.Player.SetQueue(Items.ToList(), false);
            await App.Player.PlayQueueAsync();
        }

        private async void ShuffleAll(object sender, EventArgs e)
        {
            await App.Player.SetQueue(Items.ToList(), true);
            await App.Player.PlayQueueAsync();
        }
    }
}

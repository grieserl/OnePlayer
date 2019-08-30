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
    public partial class ArtistListPage : ContentPage
    {
        public ObservableCollection<Artist> Items { get; set; }

        public ArtistListPage()
        {
            InitializeComponent();

            Items = new ObservableCollection<Artist>();
  

            MyListView.ItemsSource = Items;
            SetList();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await Navigation.PushAsync(new TrackListPage((Artist)e.Item));
            

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }             

        public async Task SetList()
        {
            Items = new ObservableCollection<Artist>(await App.Library.GetArtists());
            MyListView.ItemsSource = Items;
        }
    }
}

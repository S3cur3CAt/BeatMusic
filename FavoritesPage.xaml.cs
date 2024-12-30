// File: FavoritesPage.xaml.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Linq;
using System.Net.NetworkInformation;

namespace BeatCat
{
    public sealed partial class FavoritesPage : Page
    {
        public FavoritesPage()
        {
            this.InitializeComponent();
            LoadFavorites();
        }

        private void LoadFavorites()
        {
            FavoritesListView.ItemsSource = AppState.Favorites;
        }

        private void FavoritesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedVideo = e.ClickedItem as Video;
            if (selectedVideo != null)
            {
                Frame.Navigate(typeof(PlayerPage), selectedVideo.Id);
            }
        }

        private void RemoveFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var video = button.DataContext as Video;
            if (video != null)
            {
                AppState.Favorites.Remove(video);
            }
        }
    }
}
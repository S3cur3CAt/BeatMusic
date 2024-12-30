// File: SearchPage.xaml.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BeatCat
{
    public sealed partial class SearchPage : Page
    {
        private YouTubeService _youTubeService;
        private ObservableCollection<Video> _searchResults;

        public SearchPage()
        {
            this.InitializeComponent();
            _youTubeService = new YouTubeService();
            _searchResults = new ObservableCollection<Video>();
            SearchResultsListView.ItemsSource = _searchResults;
        }

        private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var results = await _youTubeService.SearchVideosAsync(args.QueryText);

            // Asegúrate de que tienes resultados antes de intentar actualizarlos
            if (results != null && results.Count > 0)
            {
                _searchResults.Clear();
                foreach (var video in results)
                {
                    _searchResults.Add(video);
                    Console.WriteLine($"Agregando video: {video.Title}");
                }
            }
            else
            {
                Console.WriteLine("No se encontraron resultados para la búsqueda.");
            }
        }

        private void SearchResultsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedVideo = e.ClickedItem as Video;
            if (selectedVideo != null)
            {
                // Navigate to PlayerPage and play the selected video
                Frame.Navigate(typeof(PlayerPage), selectedVideo.Id);
            }
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var queryText = SearchBox.Text;
            if (!string.IsNullOrWhiteSpace(queryText))
            {
                // Llama al método SearchBox_QuerySubmitted directamente con el texto de la consulta.
                await SearchBox_QuerySubmitted(SearchBox, queryText);
            }
        }

        private async Task SearchBox_QuerySubmitted(AutoSuggestBox sender, string queryText)
        {
            if (!string.IsNullOrWhiteSpace(queryText))
            {
                var results = await _youTubeService.SearchVideosAsync(queryText);

                // Asegúrate de que tienes resultados antes de intentar actualizarlos
                if (results != null && results.Count > 0)
                {
                    _searchResults.Clear();
                    foreach (var video in results)
                    {
                        _searchResults.Add(video);
                        Console.WriteLine($"Agregando video: {video.Title}");
                    }
                }
                else
                {
                    Console.WriteLine("No se encontraron resultados para la búsqueda.");
                }
            }
        }

    }
}
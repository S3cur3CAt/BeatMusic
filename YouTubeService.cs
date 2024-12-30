    // File: YouTubeService.cs
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Text.Json;

    namespace BeatCat
    {
        public class YouTubeService
        {
            private const string API_KEY = "AIzaSyAZkbe86xDoObqauvT7-6BRjx7nEYXzSsk";
            private const string BASE_URL = "https://www.googleapis.com/youtube/v3";

            private readonly HttpClient _httpClient;

            public YouTubeService()
            {
                _httpClient = new HttpClient();
            }

        public async Task<List<Video>> SearchVideosAsync(string query)
        {
            var url = $"{BASE_URL}/search?part=snippet&q={Uri.EscapeDataString(query)}&type=video&key={API_KEY}";
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
                var searchResult = JsonSerializer.Deserialize<YouTubeSearchResult>(responseContent);

                if (searchResult?.Items == null || searchResult.Items.Count == 0)
                {
                    Console.WriteLine("No se encontraron videos.");
                    return new List<Video>();
                }

                var videos = new List<Video>();
                foreach (var item in searchResult.Items)
                {
                    if (item?.Id?.VideoId != null && item?.Snippet != null)
                    {
                        videos.Add(new Video
                        {
                            Id = item.Id.VideoId,
                            Title = item.Snippet.Title,
                            ChannelTitle = item.Snippet.ChannelTitle,
                            ThumbnailUrl = item.Snippet.Thumbnails?.Medium?.Url
                        });
                    }
                }
                Console.WriteLine($"Se encontraron {videos.Count} videos.");
                return videos;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error de solicitud HTTP: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                throw;
            }
        }


    }

    public class YouTubeSearchResult
        {
            public List<YouTubeSearchItem> Items { get; set; }
        }

        public class YouTubeSearchItem
        {
            public YouTubeVideoId Id { get; set; }
            public YouTubeSnippet Snippet { get; set; }
        }

        public class YouTubeVideoId
        {
            public string VideoId { get; set; }
        }

        public class YouTubeSnippet
        {
            public string Title { get; set; }
            public string ChannelTitle { get; set; }
            public YouTubeThumbnails Thumbnails { get; set; }
        }

        public class YouTubeThumbnails
        {
            public YouTubeThumbnail Medium { get; set; }
        }

        public class YouTubeThumbnail
        {
            public string Url { get; set; }
        }
    }
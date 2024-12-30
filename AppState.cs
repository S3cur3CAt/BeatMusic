// File: AppState.cs
using System.Collections.ObjectModel;

namespace BeatCat
{
    public static class AppState
    {
        public static ObservableCollection<Video> Favorites { get; } = new ObservableCollection<Video>();
        public static ObservableCollection<Video> RecentlyPlayed { get; } = new ObservableCollection<Video>();

        public static void AddToFavorites(Video video)
        {
            if (!Favorites.Contains(video))
            {
                Favorites.Add(video);
            }
        }

        public static void AddToRecentlyPlayed(Video video)
        {
            if (RecentlyPlayed.Contains(video))
            {
                RecentlyPlayed.Remove(video);
            }
            RecentlyPlayed.Insert(0, video);

            if (RecentlyPlayed.Count > 10)
            {
                RecentlyPlayed.RemoveAt(RecentlyPlayed.Count - 1);
            }
        }
    }
}
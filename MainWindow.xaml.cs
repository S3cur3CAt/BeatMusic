// File: MainWindow.xaml.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.Search;

namespace BeatCat
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            NavView.SelectedItem = NavView.MenuItems[0];
            ContentFrame.Navigate(typeof(PlayerPage));
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                var navItemTag = args.SelectedItemContainer.Tag.ToString();
                switch (navItemTag)
                {
                    case "player":
                        ContentFrame.Navigate(typeof(PlayerPage));
                        break;
                    case "search":
                        ContentFrame.Navigate(typeof(SearchPage));
                        break;
                    case "favorites":
                        ContentFrame.Navigate(typeof(FavoritesPage));
                        break;
                }
            }
        }
    }
}

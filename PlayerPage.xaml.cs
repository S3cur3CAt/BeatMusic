using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.Web.WebView2.Core;
using System;
using System.Threading.Tasks;

namespace BeatCat
{
    public sealed partial class PlayerPage : Page
    {
        private YouTubeService _youTubeService;
        private bool _isPlaying = false;
        private bool _isMuted = false;
        private double _currentVolume = 100;
        private bool isProgressSliderFocused = false;
        private bool isVolumeSliderFocused = false;

        public PlayerPage()
        {
            this.InitializeComponent();
            _youTubeService = new YouTubeService();
            _ = InitializeYouTubePlayerAsync();
        }

        private async Task InitializeYouTubePlayerAsync()
        {
            try
            {
                await InitializeYouTubePlayer();
            }
            catch (Exception ex)
            {
                // Manejo de errores
            }
        }

        private async Task InitializeYouTubePlayer()
        {
            try
            {
                await YoutubePlayer.EnsureCoreWebView2Async();
                YoutubePlayer.CoreWebView2.Navigate("about:blank");
                YoutubePlayer.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CoreWebView2_WebMessageReceived(CoreWebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            var message = System.Text.Json.JsonSerializer.Deserialize<PlayerStateMessage>(args.WebMessageAsJson);
            if (message.type == "stateChange")
            {
                _isPlaying = message.data == 1;
                UpdatePlayPauseButton();
            }
        }

        // Método asíncrono para manejar la reproducción/pausa
        private async Task HandlePlayPauseAsync()
        {
            try
            {
                if (_isPlaying)
                {
                    await YoutubePlayer.CoreWebView2.ExecuteScriptAsync("player.pauseVideo();");
                }
                else
                {
                    await YoutubePlayer.CoreWebView2.ExecuteScriptAsync("player.playVideo();");
                }
                _isPlaying = !_isPlaying;
                UpdatePlayPauseButton();
            }
            catch (Exception ex)
            {
                // Manejo de errores
            }
        }

        // Event handler del botón play/pause
        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            _ = HandlePlayPauseAsync();
        }

        // Método asíncrono para manejar el mute
        private async Task HandleMuteAsync()
        {
            try
            {
                if (_isMuted)
                {
                    await YoutubePlayer.CoreWebView2.ExecuteScriptAsync("player.unMute();");
                    await YoutubePlayer.CoreWebView2.ExecuteScriptAsync($"player.setVolume({_currentVolume});");
                }
                else
                {
                    await YoutubePlayer.CoreWebView2.ExecuteScriptAsync("player.mute();");
                }
                _isMuted = !_isMuted;
                UpdateMuteButton();
            }
            catch (Exception ex)
            {
                // Manejo de errores
            }
        }

        // Event handler del botón mute
        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            _ = HandleMuteAsync();
        }

        private void UpdatePlayPauseButton()
        {
            PlayPauseButton.Content = _isPlaying ? "\uE769" : "\uE768";
        }

        private void UpdateMuteButton()
        {
            MuteButton.Content = _isMuted ? "\uE74F" : "\uE767";
        }

        // Manejo del slider de progreso
        private async void ProgressSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (isProgressSliderFocused)
            {
                try
                {
                    await YoutubePlayer.CoreWebView2.ExecuteScriptAsync($"player.seekTo({e.NewValue}, true);");
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                }
            }
        }

        // Manejo del slider de volumen
        private async void VolumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (isVolumeSliderFocused)
            {
                try
                {
                    _currentVolume = e.NewValue;
                    await YoutubePlayer.CoreWebView2.ExecuteScriptAsync($"player.setVolume({e.NewValue});");
                }
                catch (Exception ex)
                {
                    // Manejo de errores
                }
            }
        }

        private async Task LoadVideoAsync(string videoId)
        {
            try
            {
                var embedHtml = $@"
        <html>
            <body style='margin:0'>
                <div id='player'></div>
                <script src='https://www.youtube.com/iframe_api'></script>
                <script>
                    var player;
                    function onYouTubeIframeAPIReady() {{
                        player = new YT.Player('player', {{
                            height: '100%',
                            width: '100%',
                            videoId: '{videoId}',
                            events: {{
                                'onReady': onPlayerReady,
                                'onStateChange': onPlayerStateChange
                            }}
                        }});
                    }}
                    function onPlayerReady(event) {{
                        event.target.playVideo();
                        updatePlayerInfo();
                    }}
                    function onPlayerStateChange(event) {{
                        window.chrome.webview.postMessage(JSON.stringify({{
                            type: 'stateChange',
                            data: event.data
                        }}));
                        updatePlayerInfo();
                    }}
                    function updatePlayerInfo() {{
                        if (player && player.getDuration) {{
                            window.chrome.webview.postMessage(JSON.stringify({{
                                type: 'playerInfo',
                                currentTime: player.getCurrentTime(),
                                duration: player.getDuration(),
                                volume: player.getVolume(),
                                muted: player.isMuted()
                            }}));
                        }}
                    }}
                    setInterval(updatePlayerInfo, 1000);
                </script>
            </body>
        </html>";

                // Llamamos a NavigateToString sin usar 'await' porque no es un método asincrónico
                YoutubePlayer.CoreWebView2.NavigateToString(embedHtml);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw;
            }
        }
    }

    public class PlayerStateMessage
    {
        public string type { get; set; }
        public int data { get; set; }
    }
}
using Microsoft.Web.WebView2.Core;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace BeatCat
{
    public static class YoutubePlayerCommands
    {
        public static async Task PlayPauseAsync(WebView2 player, bool isPlaying)
        {
            try
            {
                if (isPlaying)
                {
                    await player.CoreWebView2.ExecuteScriptAsync("player.pauseVideo();");
                }
                else
                {
                    await player.CoreWebView2.ExecuteScriptAsync("player.playVideo();");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task MuteUnmuteAsync(WebView2 player, bool isMuted, double currentVolume)
        {
            try
            {
                if (isMuted)
                {
                    await player.CoreWebView2.ExecuteScriptAsync("player.unMute();");
                    await player.CoreWebView2.ExecuteScriptAsync($"player.setVolume({currentVolume});");
                }
                else
                {
                    await player.CoreWebView2.ExecuteScriptAsync("player.mute();");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task SetVolumeAsync(WebView2 player, double volume)
        {
            try
            {
                await player.CoreWebView2.ExecuteScriptAsync($"player.setVolume({volume});");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task SetProgressAsync(WebView2 player, double value)
        {
            try
            {
                await player.CoreWebView2.ExecuteScriptAsync($"player.seekTo({value}, true);");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MonoGame.Framework;
using Windows.ApplicationModel.Activation;
using Windows.UI.ApplicationSettings;
using System.Diagnostics;

namespace Lumberjack
{
    /// <summary>
    /// The root page used to display the game.
    /// </summary>
    public sealed partial class GamePage : SwapChainBackgroundPanel
    {
        readonly Game1 _game;

        public GamePage(LaunchActivatedEventArgs args)
        {
            this.InitializeComponent();

            SettingsPane.GetForCurrentView().CommandsRequested += GamePage_CommandsRequested;

            // Create the game.
            _game = XamlGame<Game1>.Create(args, Window.Current.CoreWindow, this);
        }

        async void GamePage_CommandsRequested(SettingsPane settingsPane, SettingsPaneCommandsRequestedEventArgs e)
        {
            try
            {
                SettingsCommand privacy = new SettingsCommand("privacy", "Privacy",
                    (handler) =>
                    {
                        Windows.System.Launcher.LaunchUriAsync(new System.Uri(@"http://unitedjaymo.com/Lumberjack-Privacy"));
                    });
                e.Request.ApplicationCommands.Add(privacy);                  
            }
            catch { }
        }
    }
}

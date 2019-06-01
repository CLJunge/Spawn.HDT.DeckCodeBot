#region Using
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Controls.Error;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Plugins;
using MahApps.Metro.Controls;
using Spawn.HDT.DeckCodeBot.ChatBot;
using Spawn.HDT.DeckCodeBot.UI.Dialogs;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
#endregion

namespace Spawn.HDT.DeckCodeBot
{
    public class DeckCodeBotPlugin : IPlugin
    {
        #region Constants
        private const double Cooldown = 1.5;
        #endregion

        #region Member Variables
        private TwitchChatBot m_bot;
        private DateTime m_dtLastResponse;

        private MenuItem miStatus;
        private ToggleSwitch m_toggle;
        #endregion

        #region Properties
        public string Name => "Deck Bot";

        public string Description => "A chat bot for twitch posts the deck code of the deck that is currently being played whenever someone enters the specified chat command.";

        public string ButtonText => "Settings...";

        public string Author => "CLJunge";

        public Version Version => new Version(Assembly.GetExecutingAssembly().GetName().Version.ToString(3));

        public MenuItem MenuItem => CreateMenuItem();
        #endregion

        #region IPlugin Methods
        public void OnButtonPress() => ShowSettingsDialog();

        public void OnLoad()
        {
        }

        public void OnUnload()
        {
            DisconnectFromTwitch();

            Configuration.Instance.Save();
        }

        public void OnUpdate()
        {
        }
        #endregion

        #region CreateMenuItem
        private MenuItem CreateMenuItem()
        {
            m_toggle = new ToggleSwitch() { OffLabel = "Disconnected" };
            m_toggle.IsCheckedChanged += OnIsCheckedChanged;

            MenuItem miMain = new MenuItem
            {
                Header = Name
            };

            miStatus = new MenuItem
            {
                Header = m_toggle,
                Icon = new Image()
                {
                    Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Spawn.HDT.DeckCodeBot;component/Resources/Off.png", UriKind.Relative))
                }
            };

            miMain.Items.Add(miStatus);

            return miMain;
        }

        #endregion

        #region Events
        #region OnIsCheckedChanged
        private void OnIsCheckedChanged(object sender, EventArgs e)
        {
            if (m_bot == null)
            {
                m_toggle.OnLabel = "Connecting...";

                ConnectToTwitch();
            }
            else
            {
                DisconnectFromTwitch();

                miStatus.Icon = new Image()
                {
                    Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Spawn.HDT.DeckCodeBot;component/Resources/Off.png", UriKind.Relative))
                };
            }
        }
        #endregion

        #region OnBotConnected
        private void OnBotConnected(object sender, EventArgs e)
        {
            Core.MainWindow.Dispatcher.Invoke(() =>
            {
                m_toggle.OnLabel = "Connected";

                miStatus.Icon = new Image()
                {
                    Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Spawn.HDT.DeckCodeBot;component/Resources/On.png", UriKind.Relative))
                };
            });
        }
        #endregion

        #region OnBotMessage
        private void OnBotMessage(object sender, MessageEventArgs e)
        {
            double dblSeconds = (DateTime.Now - m_dtLastResponse).TotalSeconds;

            if (dblSeconds > Cooldown && e.Message.Contains(Configuration.Instance.ChatCommand))
            {
                Deck activeDeck = DeckList.Instance.ActiveDeck.GetSelectedDeckVersion();

                HearthDb.Deckstrings.Deck dbDeck = HearthDbConverter.ToHearthDbDeck(activeDeck);

                string strDeckCode = HearthDb.Deckstrings.DeckSerializer.Serialize(dbDeck, false);

                m_bot.SendPublicChatMessage($"@{e.Sender} {activeDeck.NameAndVersion} {activeDeck.GetClass}: {strDeckCode}");

                m_dtLastResponse = DateTime.Now;
            }
        }
        #endregion

        #region OnBotError
        private void OnBotError(object sender, ChatBot.ErrorEventArgs e)
        {
            m_bot?.Dispose();

            m_toggle.IsChecked = false;

            ErrorManager.AddError(new Error(Name, e.ErrorMessage), true);
        }
        #endregion
        #endregion

        #region ConnectToTwitch
        private void ConnectToTwitch()
        {
            m_bot = new TwitchChatBot(Configuration.Instance.BotUsername, Configuration.Instance.ChannelName);

            m_bot.ConnectedToChannel += OnBotConnected;
            m_bot.NewMessage += OnBotMessage;
            m_bot.Error += OnBotError;

            m_bot.Connect(Configuration.Instance.TwitchIrcUrl, Configuration.Instance.TwitchIrcPort, Configuration.Instance.TwitchOAuthToken);
        }
        #endregion

        #region DisconnectFromTwitch
        private void DisconnectFromTwitch()
        {
            if (m_bot != null)
            {
                m_bot.ConnectedToChannel -= OnBotConnected;
                m_bot.NewMessage -= OnBotMessage;
                m_bot.Error -= OnBotError;

                m_bot.Dispose();

                m_bot = null;
            }
        }
        #endregion

        #region ShowSettingsDialog
        private void ShowSettingsDialog()
        {
            SettingsDialogView settingsDialogView = new SettingsDialogView()
            {
                Owner = Core.MainWindow
            };

            settingsDialogView.ShowDialog();
        }
        #endregion

        #region GetDataDirectory
        public static string GetDataDirectory(string strFolder = "")
        {
            string strRet = Path.Combine(Config.Instance.DataDir, "DeckBot", strFolder);

            if (!Directory.Exists(strRet))
                Directory.CreateDirectory(strRet);

            return strRet;
        }
        #endregion
    }
}
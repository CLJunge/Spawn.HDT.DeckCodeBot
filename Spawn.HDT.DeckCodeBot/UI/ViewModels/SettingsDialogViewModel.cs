#region Using
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DeckCodeBot.UI.ViewModels
{
    public class SettingsDialogViewModel : ViewModelBase
    {
        #region Member Variables
        private readonly Visibility m_debugVisibility;

        private string m_strWindowTitle;
        private string m_strChannelName;
        private string m_strChannelNameLabelText;
        private string m_strBotUsername;
        private string m_strBotUsernameLabelText;
        private string m_strTwitchOAuthToken;
        private string m_strTwitchOAuthTokenLabelText;
        private string m_strTwitchIrcUrl;
        private string m_strTwitchIrcUrlLabelText;
        private int m_nTwitchIrcPort;
        private string m_strTwitchIrcPortLabelText;
        private string m_strChatCommand;
        private string m_strChatCommandLabelText;
        private bool m_blnEnableInput;
        #endregion

        #region Properties
        #region DebugVisibility
        public Visibility DebugVisibility => m_debugVisibility;
        #endregion

        #region CanNotifyDirtyStatus
        public override bool CanNotifyDirtyStatus => true;
        #endregion

        #region WindowTitle
        public string WindowTitle
        {
            get => m_strWindowTitle;
            set => Set(ref m_strWindowTitle, value);
        }
        #endregion

        #region ChannelName
        public string ChannelName
        {
            get => m_strChannelName;
            set => Set(ref m_strChannelName, value);
        }
        #endregion

        #region ChannelNameLabelText
        public string ChannelNameLabelText
        {
            get => m_strChannelNameLabelText;
            set => Set(ref m_strChannelNameLabelText, value);
        }
        #endregion

        #region BotUsername
        public string BotUsername
        {
            get => m_strBotUsername;
            set => Set(ref m_strBotUsername, value);
        }
        #endregion

        #region BotUsernameLabelText
        public string BotUsernameLabelText
        {
            get => m_strBotUsernameLabelText;
            set => Set(ref m_strBotUsernameLabelText, value);
        }
        #endregion

        #region TwitchOAuthToken
        public string TwitchOAuthToken
        {
            get => m_strTwitchOAuthToken;
            set => Set(ref m_strTwitchOAuthToken, value);
        }
        #endregion

        #region TwitchOAuthTokenLabelText
        public string TwitchOAuthTokenLabelText
        {
            get => m_strTwitchOAuthTokenLabelText;
            set => Set(ref m_strTwitchOAuthTokenLabelText, value);
        }
        #endregion

        #region TwitchIrcUrl
        public string TwitchIrcUrl
        {
            get => m_strTwitchIrcUrl;
            set => Set(ref m_strTwitchIrcUrl, value);
        }
        #endregion

        #region TwitchIrcUrlLabelText
        public string TwitchIrcUrlLabelText
        {
            get => m_strTwitchIrcUrlLabelText;
            set => Set(ref m_strTwitchIrcUrlLabelText, value);
        }
        #endregion

        #region TwitchIrcPort
        public int TwitchIrcPort
        {
            get => m_nTwitchIrcPort;
            set => Set(ref m_nTwitchIrcPort, value);
        }
        #endregion

        #region TwitchIrcPortLabelText
        public string TwitchIrcPortLabelText
        {
            get => m_strTwitchIrcPortLabelText;
            set => Set(ref m_strTwitchIrcPortLabelText, value);
        }
        #endregion

        #region ChatCommand
        public string ChatCommand
        {
            get => m_strChatCommand;
            set => Set(ref m_strChatCommand, value);
        }
        #endregion

        #region ChatCommandLabelText
        public string ChatCommandLabelText
        {
            get => m_strChatCommandLabelText;
            set => Set(ref m_strChatCommandLabelText, value);
        }
        #endregion

        #region EnableInput
        public bool EnableInput
        {
            get => m_blnEnableInput;
            set => Set(ref m_blnEnableInput, value);
        }
        #endregion

        #region SaveSettingsCommand
        public ICommand SaveSettingsCommand => new RelayCommand(SaveSettings, () => IsDirty);
        #endregion
        #endregion

        #region Ctor
        public SettingsDialogViewModel()
        {
#if DEBUG
            m_debugVisibility = Visibility.Visible;
#else
            m_debugVisibility = Visibility.Collapsed;
#endif

            WindowTitle = "Deck Bot - Settings";

            LoadLabelTexts();

            LoadValues();

            NotifyDirtyStatus += OnNotifyDirtyStatus;
        }
        #endregion

        #region OnNotifyDirtyStatus
        private void OnNotifyDirtyStatus(object sender, NotifyDirtyStatusEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ChannelName):
                    if (e.IsDirty && !ChannelNameLabelText.EndsWith(IsDirtySuffix))
                    {
                        ChannelNameLabelText = $"{ChannelNameLabelText}{IsDirtySuffix}";
                    }
                    else if (!e.IsDirty && ChannelNameLabelText.EndsWith(IsDirtySuffix))
                    {
                        ChannelNameLabelText = ChannelNameLabelText.Substring(0, ChannelNameLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(BotUsername):
                    if (e.IsDirty && !BotUsernameLabelText.EndsWith(IsDirtySuffix))
                    {
                        BotUsernameLabelText = $"{BotUsernameLabelText}{IsDirtySuffix}";
                    }
                    else if (!e.IsDirty && BotUsernameLabelText.EndsWith(IsDirtySuffix))
                    {
                        BotUsernameLabelText = BotUsernameLabelText.Substring(0, BotUsernameLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(TwitchOAuthToken):
                    if (e.IsDirty && !TwitchOAuthTokenLabelText.EndsWith(IsDirtySuffix))
                    {
                        TwitchOAuthTokenLabelText = $"{TwitchOAuthTokenLabelText}{IsDirtySuffix}";
                    }
                    else if (!e.IsDirty && TwitchOAuthTokenLabelText.EndsWith(IsDirtySuffix))
                    {
                        TwitchOAuthTokenLabelText = TwitchOAuthTokenLabelText.Substring(0, TwitchOAuthTokenLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(TwitchIrcUrl):
                    if (e.IsDirty && !TwitchIrcUrlLabelText.EndsWith(IsDirtySuffix))
                    {
                        TwitchIrcUrlLabelText = $"{TwitchIrcUrlLabelText}{IsDirtySuffix}";
                    }
                    else if (!e.IsDirty && TwitchIrcUrlLabelText.EndsWith(IsDirtySuffix))
                    {
                        TwitchIrcUrlLabelText = TwitchIrcUrlLabelText.Substring(0, TwitchIrcUrlLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(TwitchIrcPort):
                    if (e.IsDirty && !TwitchIrcPortLabelText.EndsWith(IsDirtySuffix))
                    {
                        TwitchIrcPortLabelText = $"{TwitchIrcPortLabelText}{IsDirtySuffix}";
                    }
                    else if (!e.IsDirty && TwitchIrcPortLabelText.EndsWith(IsDirtySuffix))
                    {
                        TwitchIrcPortLabelText = TwitchIrcPortLabelText.Substring(0, TwitchIrcPortLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;

                case nameof(ChatCommand):
                    if (e.IsDirty && !ChatCommandLabelText.EndsWith(IsDirtySuffix))
                    {
                        ChatCommandLabelText = $"{ChatCommandLabelText}{IsDirtySuffix}";
                    }
                    else if (!e.IsDirty && ChatCommandLabelText.EndsWith(IsDirtySuffix))
                    {
                        ChatCommandLabelText = ChatCommandLabelText.Substring(0, ChatCommandLabelText.Length - IsDirtySuffix.Length);
                    }
                    break;
            }
        }
        #endregion

        #region SaveSettings
        private void SaveSettings()
        {
            Configuration.Instance.ChannelName = ChannelName;
            Configuration.Instance.BotUsername = BotUsername;
            Configuration.Instance.TwitchOAuthToken = TwitchOAuthToken;
            Configuration.Instance.TwitchIrcUrl = TwitchIrcUrl;
            Configuration.Instance.TwitchIrcPort = TwitchIrcPort;
            Configuration.Instance.ChatCommand = ChatCommand;

            Configuration.Instance.Save();

            Debug.WriteLine("Saved config");
        }
        #endregion

        #region LoadLabelTexts
        private void LoadLabelTexts()
        {
            ChannelNameLabelText = "Channel name";
            BotUsernameLabelText = "Bot username";
            TwitchOAuthTokenLabelText = "OAuth token";
            TwitchIrcUrlLabelText = "IRC URL";
            TwitchIrcPortLabelText = "IRC port";
            ChatCommandLabelText = "Chat command";
        }
        #endregion

        #region LoadValues
        private void LoadValues()
        {
            ChannelName = Configuration.Instance.ChannelName;
            BotUsername = Configuration.Instance.BotUsername;
            TwitchOAuthToken = Configuration.Instance.TwitchOAuthToken;
            TwitchIrcUrl = Configuration.Instance.TwitchIrcUrl;
            TwitchIrcPort = Configuration.Instance.TwitchIrcPort;
            ChatCommand = Configuration.Instance.ChatCommand;

            SetInitialPropertyValue(nameof(ChannelName), ChannelName);
            SetInitialPropertyValue(nameof(BotUsername), BotUsername);
            SetInitialPropertyValue(nameof(TwitchOAuthToken), TwitchOAuthToken);
            SetInitialPropertyValue(nameof(TwitchIrcUrl), TwitchIrcUrl);
            SetInitialPropertyValue(nameof(TwitchIrcPort), TwitchIrcPort);
            SetInitialPropertyValue(nameof(ChatCommand), ChatCommand);
        }
        #endregion
    }
}
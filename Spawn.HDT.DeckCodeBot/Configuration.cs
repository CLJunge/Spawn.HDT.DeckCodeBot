#region Using
using System.ComponentModel;
using System.IO;
#endregion

namespace Spawn.HDT.DeckCodeBot
{
    public class Configuration : ObservableObject
    {
        #region Instance
        private static Configuration s_instance;

        public static Configuration Instance => s_instance ?? (s_instance = Load());
        #endregion

        #region Member Variables
        private string m_strChannelName;
        private string m_strBotUsername;
        private string m_strTwitchOAuthToken;
        private string m_strTwitchIrcUrl;
        private int m_nTwitchIrcPort;
        private string m_strChatCommand;
        #endregion

        #region Properties
        #region ChannelName
        [DefaultValue("")]
        public string ChannelName
        {
            get => m_strChannelName;
            set => Set(ref m_strChannelName, value);
        }
        #endregion

        #region BotUserName
        [DefaultValue("")]
        public string BotUsername
        {
            get => m_strBotUsername;
            set => Set(ref m_strBotUsername, value);
        }
        #endregion

        #region TwitchOAuthToken
        [DefaultValue("")]
        public string TwitchOAuthToken
        {
            get => m_strTwitchOAuthToken;
            set => Set(ref m_strTwitchOAuthToken, value);
        }
        #endregion

        #region TwitchIrcUrl
        [DefaultValue("irc.twitch.tv")]
        public string TwitchIrcUrl
        {
            get => m_strTwitchIrcUrl;
            set => Set(ref m_strTwitchIrcUrl, value);
        }
        #endregion

        #region TwitchIrcPort
        [DefaultValue(6667)]
        public int TwitchIrcPort
        {
            get => m_nTwitchIrcPort;
            set => Set(ref m_nTwitchIrcPort, value);
        }
        #endregion

        #region ChatCommand
        [DefaultValue("!deck")]
        public string ChatCommand
        {
            get => m_strChatCommand;
            set => Set(ref m_strChatCommand, value);
        }
        #endregion

        #region IsValid
        public bool IsValid => !string.IsNullOrEmpty(ChannelName) && !string.IsNullOrEmpty(BotUsername)
            && !string.IsNullOrEmpty(TwitchOAuthToken) && !string.IsNullOrEmpty(TwitchIrcUrl) && !string.IsNullOrEmpty(ChatCommand);
        #endregion
        #endregion

        #region Ctor
        public Configuration()
        {
            ChannelName = "";
            BotUsername = "";
            TwitchOAuthToken = "";
            TwitchIrcUrl = "irc.twitch.tv";
            TwitchIrcPort = 6667;
            ChatCommand = "!deck";
        }
        #endregion

        #region Save
        public void Save(string strFileName = "config.xml") => FileHelper.Write(Path.Combine(DeckCodeBotPlugin.GetDataDirectory(), strFileName), this);
        #endregion

        #region [STATIC] Load
        public static Configuration Load(string strFileName = "config.xml") => FileHelper.Read<Configuration>(Path.Combine(DeckCodeBotPlugin.GetDataDirectory(), strFileName));
        #endregion
    }
}
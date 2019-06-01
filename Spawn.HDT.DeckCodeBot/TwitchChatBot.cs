#region Using
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
#endregion

namespace Spawn.HDT.DeckCodeBot
{
    //Based on https://liquid.fish/current/twitch-irc-bot-c-console-application
    public class TwitchChatBot : IDisposable
    {
        #region Constants
        private const string WelcomeMessage = "Welcome, GLHF!";
        #endregion

        #region Custom Events
        public event EventHandler ConnectedToChannel;
        public event EventHandler<MessageEventArgs> NewMessage;
        public event EventHandler<ErrorEventArgs> Error;
        #endregion

        #region Member Variables
        private TcpClient m_client;
        private StreamReader m_reader;
        private StreamWriter m_writer;
        #endregion

        #region Properties
        public string Username { get; }
        public string Channel { get; }
        public bool Running { get; private set; }
        #endregion

        #region Ctor
        public TwitchChatBot(string username, string channel)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");

            if (string.IsNullOrEmpty(channel))
                throw new ArgumentNullException("channel");

            Username = username;
            Channel = channel;
        }
        #endregion

        #region Connect
        public void Connect(string strUrl, int nPort, string strOAuthToken)
        {
            if (!Running && !string.IsNullOrEmpty(strUrl)
                && !string.IsNullOrEmpty(strOAuthToken))
            {
                try
                {
                    Debug.WriteLine($"Connecting to {strUrl}:{nPort} (#{Channel})...");

                    m_client = new TcpClient(strUrl, nPort);

                    Stream stream = m_client.GetStream();

                    m_reader = new StreamReader(stream);
                    m_writer = new StreamWriter(stream);

                    Join(strOAuthToken);

                    Running = true;

                    Task.Factory.StartNew(async () => await KeepAlive());
                    Task.Factory.StartNew(async () => await ReadMessages());
                }
                catch (Exception ex)
                {
                    string strErrorMessage = $"Couldn't connect to Twitch IRC: {ex.Message}";

                    Debug.WriteLine(strErrorMessage);

                    Error.Invoke(this, new ErrorEventArgs(strErrorMessage));
                }
            }
        }
        #endregion

        #region SendIrcMessage
        public void SendIrcMessage(string strMessage)
        {
            try
            {
                m_writer?.WriteLine(strMessage);
                m_writer?.Flush();
            }
            catch (Exception ex)
            {
                string strErrorMessage = $"Error sending irc message: {ex.Message}";

                Debug.WriteLine(strErrorMessage);

                Error?.Invoke(this, new ErrorEventArgs(strErrorMessage));
            }
        }
        #endregion

        #region SendPublicChatMessage
        public void SendPublicChatMessage(string strMessage) => SendIrcMessage($":{Username}!{Username}@{Username}.tmi.twitch.tv PRIVMSG #{Channel} :{strMessage}");
        #endregion

        #region ReadMessages
        public async Task ReadMessages()
        {
            Debug.WriteLine("Started ReadMessages task...");

            while (Running)
            {
                try
                {
                    string strMessage = m_reader.ReadLine();

                    if (!string.IsNullOrEmpty(strMessage))
                    {
                        Debug.WriteLine($"New message: \"{strMessage}\"");

                        string strChatMessage = GetChatMessage(strMessage);
                        string strSender = GetSender(strMessage.Replace(strChatMessage, string.Empty));

                        NewMessage?.Invoke(this, new MessageEventArgs(strSender, strMessage));

                        if (strMessage.Contains(WelcomeMessage))
                            ConnectedToChannel?.Invoke(this, EventArgs.Empty);

                        await Task.Delay(10);
                    }
                }
                catch (Exception ex)
                {
                    string strErrorMessage = $"Error receiving message: {ex.Message}";

                    Debug.WriteLine(strErrorMessage);

                    Error?.Invoke(this, new ErrorEventArgs(strErrorMessage));
                }
            }
        }
        #endregion

        #region GetSender
        private string GetSender(string strMessage)
        {
            string strRet = string.Empty;

            string[] vTemp = strMessage.Split('!');

            if (vTemp.Length > 1)
            {
                strRet = vTemp[0];

                strRet = strRet.Substring(1, strRet.Length - 1);
            }

            return strRet;
        }
        #endregion

        #region GetChatMessage
        private string GetChatMessage(string strMessage)
        {
            string[] vTemp = strMessage.Split(':');

            return vTemp[vTemp.Length - 1];
        }
        #endregion

        #region KeepAlive
        private async Task KeepAlive()
        {
            Debug.WriteLine("Started KeepAlive task...");

            while (Running)
            {
                Debug.WriteLine("Sending ping to Twitch...");

                SendIrcMessage("PING irc.twitch.tv");

                await Task.Delay(300000); // 5 minutes
            }
        }
        #endregion

        #region Join
        private void Join(string strTwitchOAuthToken)
        {
            Debug.WriteLine($"Joining #{Channel} as {Username}...");

            m_writer.WriteLine($"PASS {strTwitchOAuthToken}");
            m_writer.WriteLine($"NICK {Username}");
            m_writer.WriteLine($"USER {Username} 8 * :{Username}");
            m_writer.WriteLine($"JOIN #{Channel}");
            m_writer.Flush();
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Running = false;

            m_writer?.Dispose();
            m_writer = null;

            m_reader?.Dispose();
            m_reader = null;

            m_client?.Close();
            m_client = null;

            Debug.WriteLine($"Closed connection");
        }
        #endregion
    }
}
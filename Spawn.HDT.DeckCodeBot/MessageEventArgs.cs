namespace Spawn.HDT.DeckCodeBot
{
    public class MessageEventArgs : System.EventArgs
    {
        #region Properties
        public string Sender { get; }

        public string Message { get; }
        #endregion

        #region Ctor
        public MessageEventArgs(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }
        #endregion
    }
}
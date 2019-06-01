namespace Spawn.HDT.DeckCodeBot.ChatBot
{
    public class ErrorEventArgs : System.EventArgs
    {
        #region Properties
        public string ErrorMessage { get; }
        #endregion

        #region Ctor
        public ErrorEventArgs(string errorMessage) => ErrorMessage = errorMessage;
        #endregion
    }
}
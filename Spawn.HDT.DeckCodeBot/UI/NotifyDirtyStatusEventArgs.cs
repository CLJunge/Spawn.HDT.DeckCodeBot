namespace Spawn.HDT.DeckCodeBot.UI
{
    public class NotifyDirtyStatusEventArgs : System.ComponentModel.PropertyChangedEventArgs
    {
        #region Properties
        public bool IsDirty { get; private set; }
        #endregion

        #region Ctor
        public NotifyDirtyStatusEventArgs(string propertyName, bool isDirty) : base(propertyName) => IsDirty = isDirty;
        #endregion
    }
}
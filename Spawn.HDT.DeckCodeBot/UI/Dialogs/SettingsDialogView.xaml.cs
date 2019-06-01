namespace Spawn.HDT.DeckCodeBot.UI.Dialogs
{
    public partial class SettingsDialogView
    {
        #region Ctor
        public SettingsDialogView() => InitializeComponent();
        #endregion

        #region Events
        #region OnOkClick
        private void OnOkClick(object sender, System.Windows.RoutedEventArgs e) => DialogResult = true;
        #endregion
        #endregion
    }
}
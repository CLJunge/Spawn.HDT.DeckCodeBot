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

        #region OnRequestNavigate
        private void OnRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(e.Uri.AbsoluteUri));

            e.Handled = true;
        }
        #endregion

        #endregion
    }
}
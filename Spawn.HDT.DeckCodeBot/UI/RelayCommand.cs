#region Using
using System;
using System.Windows.Input;
#endregion

namespace Spawn.HDT.DeckCodeBot.UI
{
    public class RelayCommand : ICommand
    {
        #region EventHandler
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #endregion

        #region Member Variables
        private readonly Action m_methodToExecute;
        private readonly Func<bool> m_canExecuteEvaluator;
        #endregion

        #region Ctor
        public RelayCommand(Action methodToExecute, Func<bool> canExecuteEvaluator)
        {
            m_methodToExecute = methodToExecute;
            m_canExecuteEvaluator = canExecuteEvaluator;
        }

        public RelayCommand(Action methodToExecute)
            : this(methodToExecute, null)
        {
        }
        #endregion

        #region CanExecute
        public bool CanExecute(object parameter) => m_canExecuteEvaluator?.Invoke() ?? true;
        #endregion

        #region Execute
        public void Execute(object parameter) => m_methodToExecute?.Invoke();
        #endregion
    }
}
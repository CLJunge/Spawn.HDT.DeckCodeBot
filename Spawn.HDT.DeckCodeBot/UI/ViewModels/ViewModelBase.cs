#region Using
using System;
using System.Collections.Generic;
#endregion

namespace Spawn.HDT.DeckCodeBot.UI.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        #region Constants
        public const string IsDirtySuffix = "*";
        #endregion

        #region Member Variables
        protected Dictionary<string, object> m_dInitialPropertyValues;
        #endregion

        #region Properties
        #region NotifyDirtyStatus
        public abstract bool CanNotifyDirtyStatus { get; }
        #endregion

        #region IsDirty
        public bool IsDirty { get; set; } = false;
        #endregion

        #region DirtyProperties
        public List<string> DirtyProperties { get; }
        #endregion
        #endregion

        #region Custom Events
        public event EventHandler<NotifyDirtyStatusEventArgs> NotifyDirtyStatus;
        #endregion

        #region Ctor
        protected ViewModelBase()
        {
            if (CanNotifyDirtyStatus)
            {
                m_dInitialPropertyValues = new Dictionary<string, object>();
                DirtyProperties = new List<string>();

                PropertyChanged += OnPropertyChanged;
            }
        }
        #endregion

        #region Events
        #region OnPropertyChanged
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (CanNotifyDirtyStatus && (m_dInitialPropertyValues?.ContainsKey(e.PropertyName) ?? false))
            {
                object objInitialValue = m_dInitialPropertyValues[e.PropertyName];
                object objNewValue = GetType().GetProperty(e.PropertyName).GetValue(this, null);
                bool blnIsDirty = !ComparePropertyValues(objInitialValue, objNewValue);

                if (blnIsDirty && (!DirtyProperties?.Contains(e.PropertyName) ?? false))
                    DirtyProperties.Add(e.PropertyName);
                else if (!blnIsDirty && (DirtyProperties?.Contains(e.PropertyName) ?? false))
                    DirtyProperties.Remove(e.PropertyName);

                IsDirty = DirtyProperties?.Count > 0;

                NotifyDirtyStatus?.Invoke(sender, new NotifyDirtyStatusEventArgs(e.PropertyName, blnIsDirty));
            }
        }
        #endregion
        #endregion

        #region SetInitialPropertyValue
        protected void SetInitialPropertyValue(string strPropertyName, object objValue)
        {
            if (CanNotifyDirtyStatus && m_dInitialPropertyValues != null)
                m_dInitialPropertyValues[strPropertyName] = objValue;
        }
        #endregion

        #region ComparePropertyValues
        protected virtual bool ComparePropertyValues<T>(T a, T b) => EqualityComparer<T>.Default.Equals(a, b);
        #endregion
    }
}
////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using Xamarin.Forms;

using Plugin.SecureStorage;

namespace SecureStorageSample.ViewModels
{
    /// <summary>
    /// View model for the app
    /// </summary>
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            SetCommand = new Command(ExecuteSetCommand);
            GetCommand = new Command(ExecuteGetCommand);
            HasCommand = new Command(ExecuteHasCommand);
            DeleteCommand = new Command(ExecuteDeleteCommand);
        }

        private string _key;
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                if (_key != value)
                {
                    _key = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _setVal;
        public string SetVal
        {
            get
            {
                return _setVal;
            }
            set
            {
                if (_setVal != value)
                {
                    _setVal = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _getVal;
        public string GetVal
        {
            get
            {
                return _getVal;
            }
            private set
            {
                if (_getVal != value)
                {
                    _getVal = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _hasVal;
        public string HasVal
        {
            get
            {
                return _hasVal;
            }
            private set
            {
                if (_hasVal != value)
                {
                    _hasVal = value;
                    RaisePropertyChanged();
                }
            }
        }

        private string _errMessage;
        public string ErrMessage
        {
            get
            {
                return _errMessage;
            }
            private set
            {
                if (_errMessage != value)
                {
                    _errMessage = value;
                    RaisePropertyChanged();
                }
            }
        }

        public ICommand GetCommand
        {
            get;
            private set;
        }

        public ICommand SetCommand
        {
            get;
            private set;
        }

        public ICommand HasCommand
        {
            get;
            private set;
        }

        public ICommand DeleteCommand
        {
            get;
            private set;
        }

        private void ExecuteSetCommand(object sender)
        {
            ErrMessage = string.Empty;
            try
            {
                CrossSecureStorage.Current.SetValue(Key, SetVal);
            }
            catch (Exception ex)
            {
                ErrMessage = ex.Message;
            }
        }

        private void ExecuteGetCommand(object sender)
        {
            ErrMessage = string.Empty;
            try
            {
                GetVal = CrossSecureStorage.Current.GetValue(Key);
            }
            catch (Exception ex)
            {
                ErrMessage = ex.Message;
            }
        }

        private void ExecuteHasCommand(object sender)
        {
            ErrMessage = string.Empty;
            try
            {
                HasVal = CrossSecureStorage.Current.HasKey(Key) ? "Y" : "N";
            }
            catch (Exception ex)
            {
                ErrMessage = ex.Message;
            }
        }

        private void ExecuteDeleteCommand(object sender)
        {
            ErrMessage = string.Empty;
            try
            {
                CrossSecureStorage.Current.DeleteKey(Key);
                GetVal = string.Empty;
                HasVal = string.Empty;
            }
            catch (Exception ex)
            {
                ErrMessage = ex.Message;
            }
        }

        private void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

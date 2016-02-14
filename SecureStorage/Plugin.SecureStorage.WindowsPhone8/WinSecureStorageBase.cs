using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

using Plugin.SecureStorage.Abstractions;

namespace Plugin.SecureStorage
{
    /// <summary>
    /// Base class for handling in memory operation for windows platform
    /// The derived classes must take care of persisting the information
    /// </summary>
    public abstract class WinSecureStorageBase : SecureStorageImplementationBase
    {
        /// <summary>
        /// Name of the storage file.
        /// </summary>
        public static string StorageFile = "Util.SecureStorage";

        /// <summary>
        /// Password for storage.
        /// Must be set prior to usage in Android and Windows
        /// </summary>
        public static string StoragePassword;

        /// <summary>
        /// Default constructor, validates settings, loads the store
        /// </summary>
        protected WinSecureStorageBase()
        {
            // verify that password is set
            if (string.IsNullOrWhiteSpace(StoragePassword))
            {
                throw new Exception($"Must set StoragePassword");
            }

            StoragePasswordArray = Encoding.UTF8.GetBytes(StoragePassword);
            // verify that storage file is set
            if (string.IsNullOrWhiteSpace(StorageFile))
            {
                throw new Exception($"Must set StorageFile");
            }

            // load from the storage
            LoadFromStorage();
        }

        #region ISecureStorage implementation
        /// <summary>
        /// Retrieves the value from storage.
        /// If value with the given key does not exist,
        /// returns default value
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public override string GetValue(string key, string defaultValue = null)
        {
            // validate usign base class
            base.GetValue(key, defaultValue);

            // if key is present, return the value
            // else return the default
            if (HasKey(key))
            {
                return _store[key];
            }

            return defaultValue;
        }

        /// <summary>
        /// Sets the value for the given key. If value exists, overwrites it
        /// Else creates new entry.
        /// Does not accept null value.
        /// </summary>
        /// <returns>true</returns>
        /// <c>false</c>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public override bool SetValue(string key, string value)
        {
            // valdiate using base class
            base.SetValue(key, value);

            // save in dictionary
            _store[key] = value;

            // update the storage
            SaveToStorage();

            return true;
        }

        /// <summary>
        /// Deletes the key and corresponding value from the storage
        /// </summary>
        /// <returns><c>true</c>, if key was deleted, <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public override bool DeleteKey(string key)
        {
            // valdiate using base class
            base.DeleteKey(key);

            // if the key exists, delete from dictionary
            // and save
            if (HasKey(key))
            {
                var ret = _store.Remove(key);

                SaveToStorage();

                return ret;
            }

            return false;
        }

        /// <summary>
        /// Determines whether specified key exists in the storage
        /// </summary>
        /// <returns><c>true</c> if this instance has key the specified key; otherwise, <c>false</c>.</returns>
        /// <param name="key">Key.</param>
        public override bool HasKey(string key)
        {
            // valdiate using base class
            base.HasKey(key);

            // return from the dictionary
            return _store.ContainsKey(key);
        }
        #endregion

        /// <summary>
        /// Loads the dictionary from storge
        /// </summary>
        private void LoadFromStorage()
        {
            // get the clear(unencrypted) data from the derived class
            byte[] clearData = LoadData();
            if (clearData != null && clearData.Length > 0)
            {
                // write the data to json serializer and convert it to dictionary.
                using (var ms = new MemoryStream())
                {
                    ms.Write(clearData, 0, clearData.Count());
                    ms.Position = 0;

                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(_store.GetType());
                    _store = (Dictionary<string, string>)serializer.ReadObject(ms);
                }
            }
        }

        /// <summary>
        /// method to encrypt and save dictionary to storage
        /// </summary>
        private void SaveToStorage()
        {
            // create serializer and memory stream to convert dictionary into json string
            // encrypt the json string and save it to storage file
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(_store.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                // serialize dictionary into mem stream
                serializer.WriteObject(ms, _store);
                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                {
                    // convert stream to string and convert it to byte array
                    var myStr = sr.ReadToEnd();
                    byte[] clearBytes = Encoding.UTF8.GetBytes(myStr);

                    // the derived class should encrypt and save the data
                    SaveData(clearBytes);
                }
            }
        }

        /// <summary>
        /// Derived classes must implement this method.
        /// Derived class will read the data from the storage, decrypt it and
        /// return clear data.
        /// </summary>
        /// <returns>Clear data</returns>
        protected abstract byte[] LoadData();

        /// <summary>
        /// Derived classes must implement this method.
        /// Derived class will encrypt the data and save it.
        /// </summary>
        /// <param name="data">Unencrypted data</param>
        protected abstract void SaveData(byte[] data);

        /// <summary>
        /// array corresponding to the password
        /// </summary>
        protected readonly byte[] StoragePasswordArray;

        /// <summary>
        /// dictionary to store values
        /// </summary>
        private Dictionary<string, string> _store = new Dictionary<string, string>();
    }
}

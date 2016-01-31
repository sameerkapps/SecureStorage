using Plugin.SecureStorage.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;


namespace Plugin.SecureStorage
{
    class SecureStorageImplementation : SecureStorageImplementationBase
    {
        /// <summary>
        /// Name of the storage file.
        /// </summary>
        public static string StorageFile = "Util.SecureStorage";

        /// <summary>
        /// Password for storage
        /// </summary>
        public static string StoragePassword;

        /// <summary>
        /// Default constructor, validates settings, loads the store
        /// </summary>
        public SecureStorageImplementation()
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
            // get storage for the app
            var storageFile = IsolatedStorageFile.GetUserStoreForApplication();

            // if file exists, open it
            if (storageFile.FileExists(StorageFile))
            {
                using (var stream = storageFile.OpenFile(StorageFile,
                                                          FileMode.Open,
                                                          FileAccess.ReadWrite))
                {
                    // allocate and read the protected data
                    byte[] protectedBytes = new byte[stream.Length];
                    stream.Read(protectedBytes, 0, (int)stream.Length);

                    // obtain clear data by decrypting
                    byte[] clearData = ProtectedData.Unprotect(protectedBytes, StoragePasswordArray);

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
        }

        /// <summary>
        /// method to encrypt and save dictionary to storage
        /// </summary>
        private void SaveToStorage()
        {
            // get storage for the app
            var storageFile = IsolatedStorageFile.GetUserStoreForApplication();

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
                    // convert stream to string and encrypt it using password
                    var myStr = sr.ReadToEnd();
                    byte[] clearBytes = Encoding.UTF8.GetBytes(myStr);
                    byte[] protectedBytes = ProtectedData.Protect(clearBytes, StoragePasswordArray);

                    // save encrypted bytes to file
                    using (var stream = storageFile.CreateFile(StorageFile))
                    {
                        stream.Write(protectedBytes, 0, protectedBytes.Count());
                        stream.Close();
                    }
                }
            }
        }

        // array corresponding to the password
        private readonly byte[] StoragePasswordArray;

        // dictionary to store values
        private Dictionary<string, string> _store = new Dictionary<string, string>();
    }

}
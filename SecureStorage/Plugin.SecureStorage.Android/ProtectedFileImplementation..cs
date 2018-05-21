////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

using Android.OS;
using Java.Security;
using Javax.Crypto;

using Plugin.SecureStorage.Abstractions;

namespace Plugin.SecureStorage
{
    /// <summary>
    /// Saves data in a password protected file
    /// </summary>
    public class ProtectedFileImplementation : ISecureStorage
    {
        #region constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ProtectedFileImplementation()
        {
            InitFileImplementation();
        }
        #endregion

        #region ISecureStorage
        /// <summary>
        /// Gets value from the password protected file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetValue(string key, string defaultValue = null)
        {
            // get the entry from the store
            // if it does not exist, return the default value
            KeyStore.SecretKeyEntry entry = GetSecretKeyEntry(key);

            if (entry != null)
            {
                var encodedBytes = entry.SecretKey.GetEncoded();
                return Encoding.UTF8.GetString(encodedBytes);
            }

            return defaultValue;
        }

        /// <summary>
        /// Sets value in the password protected file
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(string key, string value)
        {
            // create entry
            var secKeyEntry = new KeyStore.SecretKeyEntry(new StringKeyEntry(value));

            // save it in the KeyStore
            _store.SetEntry(key, secKeyEntry, _passwordProtection);

            // save the store
            Save();

            return true;
        }

        /// <summary>
        /// Deletes a value from the password protected file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DeleteKey(string key)
        {
            // retrieve the entry
            KeyStore.SecretKeyEntry entry = GetSecretKeyEntry(key);

            // if entry exists, delete from store, save the store and return true
            if (entry != null)
            {
                _store.DeleteEntry(key);

                Save();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks, if it has the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasKey(string key)
        {
            // retrieve to see, if it exists
            return GetSecretKeyEntry(key) != null;
        }
        #endregion

        #region static properties
        /// <summary>
        /// Name of the storage file.
        /// </summary>
        public static string StorageFile = "Util.SecureStorage";

        /// <summary>
        /// Password for storage. The default value is the serial number of the hardware of the device.
        /// It is expected to be unique per device. But can be found out from the device.
        /// To prevent it, assign your own password and obfuscate the app.
        /// </summary>
        public static string StoragePassword = Build.Serial;
        #endregion

        #region  PasswordProtectedFile
        /// <summary>
        /// Initialization for file implementation
        /// </summary>
        private void InitFileImplementation()
        {
            // verify that password is set
            if (string.IsNullOrWhiteSpace(StoragePassword))
            {
                throw new Exception($"Must set StoragePassword");
            }

            StoragePasswordArray = StoragePassword.ToCharArray();

            // Instantiate store and protection
            _store = KeyStore.GetInstance(KeyStore.DefaultType);
            _passwordProtection = new KeyStore.PasswordProtection(StoragePasswordArray);

            // if store exists, load it from the file
            try
            {
                using (var stream = new IsolatedStorageFileStream(StorageFile, FileMode.Open, FileAccess.Read))
                {
                    _store.Load(stream, StoragePasswordArray);
                }
            }
            catch (Exception)
            {
                // this will happen for the first run. As no file is expected to be present
                _store.Load(null, StoragePasswordArray);
            }
        }

        // persists the store using password
        private void Save()
        {
            using (var stream = new IsolatedStorageFileStream(StorageFile, FileMode.OpenOrCreate, FileAccess.Write))
            {
                _store.Store(stream, StoragePasswordArray);
            }
        }

        // retrieves the secret key entry from the store
        private KeyStore.SecretKeyEntry GetSecretKeyEntry(string key)
        {
            try
            {
                return _store.GetEntry(key, _passwordProtection) as KeyStore.SecretKeyEntry;
            }
            catch (UnrecoverableKeyException) // swallow this exception. Can be caused by invalid key
            {
                return null;
            }
        }

        /// <summary>
        /// Class for storing string as entry
        /// </summary>
        private class StringKeyEntry : Java.Lang.Object, ISecretKey
        {
            private const string AlgoName = "RAW";

            private byte[] _bytes;

            /// <summary>
            /// Constructor makes sure that entry is valid.
            /// Converts it to bytes
            /// </summary>
            /// <param name="entry">Entry.</param>
            public StringKeyEntry(string entry)
            {
                if (entry == null)
                {
                    throw new ArgumentNullException();
                }

                _bytes = ASCIIEncoding.UTF8.GetBytes(entry);
            }

            #region IKey implementation
            public byte[] GetEncoded()
            {
                return _bytes;
            }

            public string Algorithm
            {
                get
                {
                    return AlgoName;
                }
            }
            public string Format
            {
                get
                {
                    return AlgoName;
                }
            }
            #endregion
        }
        #endregion

        #region fields
        private char[] StoragePasswordArray;

        // Store for Key Value pairs
        KeyStore _store;
        // password protection for the store
        KeyStore.PasswordProtection _passwordProtection;
        #endregion
    }
}
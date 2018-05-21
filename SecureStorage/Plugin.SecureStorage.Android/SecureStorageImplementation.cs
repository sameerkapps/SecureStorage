////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;

using Plugin.SecureStorage.Abstractions;

namespace Plugin.SecureStorage
{
    /// <summary>
    /// Android implementation of secure storage. Done using KeyStore
    /// Make sure to initialize store password for Android.
    /// </summary>
    public partial class SecureStorageImplementation : SecureStorageImplementationBase
  {
        #region static fields
        /// <summary>
        /// Declare the storage type
        /// </summary>
        public static StorageTypes StorageType = StorageTypes.AndroidKeyStore;
        #endregion

        #region constructor
        /// <summary>
        /// Default constructor created or loads the store
        /// </summary>
        public SecureStorageImplementation()
        {
            if (StorageType == StorageTypes.AndroidKeyStore)
            {
                _implementation = new AndroidKeyStoreImplementation();
            }
            else
            {
                _implementation = new ProtectedFileImplementation();
            }
        } 
        #endregion

        #region ISecureStorage implementation
        /// <summary>
        /// Retrieves the value from storage.
        /// If value with the given key does not exist,
        /// returns default value
        /// </summary>
        /// <returns>The value.</returns>
        /// <param name="key">Key.</param>
        /// <param name="defaultValue">Default value.</param>
        public override string GetValue(string key, string defaultValue)
        {
            // validate using base class
            base.GetValue(key, defaultValue);

            return _implementation.GetValue(key, defaultValue);
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
            // validate the parameters
            base.SetValue(key, value);

            return _implementation.SetValue(key, value);
        }

        /// <summary>
        /// Deletes the key and corresponding value from the storage
        /// </summary>
        public override bool DeleteKey(string key)
        {
            // valdiate using base class
            base.DeleteKey(key);

            return _implementation.DeleteKey(key);
        }

        /// <summary>
        /// Determines whether specified key exists in the storage
        /// </summary>
        public override bool HasKey(string key)
        {
            // validate if key is valid
            base.HasKey(key);

            return _implementation.HasKey(key);
        }
        #endregion

        #region fields
        /// <summary>
        /// The actual implementation as chosen by the developer
        /// Password protected file or AndroidKeyStore
        /// </summary>
        private ISecureStorage _implementation;
        #endregion


    }
}
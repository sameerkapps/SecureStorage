////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Security.Credentials;
using Plugin.SecureStorage.Abstractions;

namespace Plugin.SecureStorage
{
    /// <summary>
    /// Secure storage implementation for UWP
    /// Known limiation - Can store max 10 values per app
    /// https://docs.microsoft.com/en-us/uwp/api/windows.security.credentials.passwordvault
    /// </summary>
    public class SecureStorageImplementation : SecureStorageImplementationBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public SecureStorageImplementation()
        {

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
            base.GetValue(key, defaultValue);

            // get the credential for the key
            var desiredCredential = GetCredential(key);

            if (desiredCredential != null)
            {
                desiredCredential.RetrievePassword();
                return desiredCredential.Password;
            }

            return defaultValue;
        }

        /// <summary>
        /// Sets the value for the given key. If value exists, removes it and add new.
        /// Effectively old value is overwrittern 
        /// Does not accept null value.
        /// </summary>
        public override bool SetValue(string key, string value)
        {
            base.SetValue(key, value);

            DeleteKey(key);

            // create credential
            try
            {
                var credential = new PasswordCredential(_credentialResource, key, value);
                _vault.Add(credential);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the key and corresponding value from the vault
        /// </summary>
        public override bool DeleteKey(string key)
        {
            // validate as per base
            base.DeleteKey(key);

            // get the credential for the key
            var desiredCredentail = GetCredential(key);
            if (desiredCredentail != null)
            {
                _vault.Remove(desiredCredentail);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether specified key exists in the vault
        /// </summary>
        public override bool HasKey(string key)
        {
            // validate as per base
            base.HasKey(key);

            return GetCredential(key) != null;
        }

        #endregion

        #region private methods
        private PasswordCredential GetCredential(string key)
        {
            // get all credentials for the resource
            var allCredential = _vault.RetrieveAll();

            // return the one for the key
            return allCredential?.FirstOrDefault(c => c.UserName == key);
        }
        #endregion

        #region private fields
        // Password vault to store data
        private readonly PasswordVault _vault = new PasswordVault();

        /// <summary>
        /// Credential resource name is same as the package
        /// </summary>
        private string _credentialResource => Package.Current.Id.Name;
        #endregion
    }
}

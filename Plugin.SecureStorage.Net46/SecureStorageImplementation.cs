using Plugin.SecureStorage.Abstractions;
using Plugin.SecureStorage.Credentials;
using System.Net;

namespace Plugin.SecureStorage
{
    /// <summary>
    /// Android implementation of secure storage. Done using KeyStore
    /// Make sure to initialize store password for Android.
    /// </summary>
    internal class SecureStorageImplementation : SecureStorageImplementationBase
    {
        private const string CredentialsUserName = "SecureStorageAgent";

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
            // validate the parameters
            base.GetValue(key, defaultValue);

            // try retrieving credential
            var output = GetCredential(key);
            if (output == null)
            {
                return defaultValue;
            }

            return output.Password;
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

            // delete previous value
            DeleteKey(key);

            try
            {
                // create entry
                var credential = new NetworkCredential(CredentialsUserName, value);
                CredentialManager.SaveCredentials(key, credential);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes the key and corresponding value from the storage
        /// </summary>
        public override bool DeleteKey(string key)
        {
            // valdiate using base class
            base.DeleteKey(key);

            // retrieve the entry
            var credential = GetCredential(key);
            if (credential == null)
            {
                return false;
            }

            // if entry exists, delete from vault and return true
            try
            {
                CredentialManager.RemoveCredentials(key);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether specified key exists in the storage
        /// </summary>
        public override bool HasKey(string key)
        {
            // validate if key is valid
            base.HasKey(key);
            // retrieve to see, if it exists
            return GetCredential(key) != null;
        }

        #endregion

        private NetworkCredential GetCredential(string key)
        {
            var output = CredentialManager.GetCredentials(key);
            return output;
        }
    }
}
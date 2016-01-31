using Plugin.SecureStorage.Abstractions;
using System;
using Security;
using Foundation;

namespace Plugin.SecureStorage
{
    /// <summary>
    /// Secure storage implementation for iOS.
    /// It is primarily for storing secure strings such as generic password.
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

        public override string GetValue(string key, string defaultValue)
        {
            base.GetValue(key, defaultValue);

            SecStatusCode ssc;
            var found = GetRecord(key, out ssc);
            if (ssc == SecStatusCode.Success)
            {
                return found.ValueData.ToString();
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

            RemoveRecord(key);
            return AddRecord(key, value) == SecStatusCode.Success;
        }

        /// <summary>
        /// Deletes the key and corresponding value from the storage
        /// </summary>
        public override bool DeleteKey(string key)
        {
            base.DeleteKey(key);

            return RemoveRecord(key) == SecStatusCode.Success;
        }

        /// <summary>
        /// Determines whether specified key exists in the storage
        /// </summary>
        public override bool HasKey(string key)
        {
            base.HasKey(key);

            SecStatusCode ssc;
            GetRecord(key, out ssc);

            // verify if it is found
            return ssc == SecStatusCode.Success;
        }

        #endregion
        /// <summary>
        /// Adds the record of type GenericPassword
        /// </summary>
        /// <returns>The record.</returns>
        /// <param name="key">Key.</param>
        /// <param name="val">Value.</param>
        private SecStatusCode AddRecord(string key, string val)
        {
            var sr = new SecRecord(SecKind.GenericPassword);
            sr.Account = key;
            sr.ValueData = NSData.FromString(val);

            return SecKeyChain.Add(sr);
        }

        /// <summary>
        /// Retreives record from the store
        /// </summary>
        /// <returns>The record.</returns>
        /// <param name="key">Key.</param>
        /// <param name="ssc">Ssc.</param>
        private SecRecord GetRecord(string key, out SecStatusCode ssc)
        {
            // create an instance of the record to query
            var sr = new SecRecord(SecKind.GenericPassword);
            sr.Account = key;
            return SecKeyChain.QueryAsRecord(sr, out ssc);
        }

        /// <summary>
        /// Removes the record.
        /// </summary>
        /// <returns>The record.</returns>
        /// <param name="key">Key.</param>
        private SecStatusCode RemoveRecord(string key)
        {
            SecStatusCode ssc;
            SecRecord found = GetRecord(key, out ssc);

            // if it exists, delete it
            if (ssc == SecStatusCode.Success)
            {
                // this has to be different that the one queried
                var sr = new SecRecord(SecKind.GenericPassword);
                sr.Account = key;
                sr.ValueData = found.ValueData;
                return SecKeyChain.Remove(sr);
            }

            return SecStatusCode.NoSuchKeyChain;
        }
    }
}
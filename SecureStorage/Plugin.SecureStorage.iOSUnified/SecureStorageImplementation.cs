////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using Security;
using Foundation;

using Plugin.SecureStorage.Abstractions;

namespace Plugin.SecureStorage
{
    /// <summary>
    /// Secure storage implementation for iOS.
    /// It is primarily for storing secure strings such as generic password.
    /// </summary>
    public class SecureStorageImplementation : SecureStorageImplementationBase
    {
        #region static props
        /// <summary>
        /// Determines when keychain info can be accessed
        /// ref: https://developer.xamarin.com/api/property/MonoTouch.Security.SecRecord.Accessible/
        /// </summary>
        public static SecAccessible DefaultAccessible = SecAccessible.AfterFirstUnlock;

        /// <summary>
        /// Enabled will store keychain entries in iCloud (if enabled on the device).
        /// Default value is false, change to true to synchronize keys
        /// </summary>
        /// <remarks>
        /// Entries are not automatically migrated between local and cloud storage when
        /// this is changed. So if you stored entries with this set to false, when
        /// you change it to true the entries will not exist in the keychain, because
        /// they will have been created as local only. 
        /// </remarks>
        public bool UseCloud { get; set; } = false;

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
            var sr = new SecRecord(StoreSecKind);
            sr.Account = key;
            sr.ValueData = NSData.FromString(val);
            if (DefaultAccessible != SecAccessible.Invalid)
            {
                sr.Accessible = DefaultAccessible;
            }

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
            var sr = new SecRecord(StoreSecKind);
            sr.Account = key;
            if (DefaultAccessible != SecAccessible.Invalid)
            {
                sr.Accessible = DefaultAccessible;
            }

            if (UseCloud)
            {
                sr.Synchronizable = true;
            }

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
                var sr = new SecRecord(StoreSecKind);
                sr.Account = key;
                sr.ValueData = found.ValueData;
                if (DefaultAccessible != SecAccessible.Invalid)
                {
                    sr.Accessible = DefaultAccessible;
                }

                if (UseCloud)
                {
                    sr.Synchronizable = true;
                }
                return SecKeyChain.Remove(sr);
            }

            return SecStatusCode.NoSuchKeyChain;
        }

#if __MAC_OS__
        // ref: https://developer.xamarin.com/api/type/MonoTouch.Security.SecKeyChain/
        // MacOS is limited to a single kind of password (SecKind.InternetPassword).
        private const SecKind StoreSecKind = SecKind.InternetPassword;
#else
        private const SecKind StoreSecKind = SecKind.GenericPassword;
#endif

    }
}
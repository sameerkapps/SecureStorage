////////////////////////////////////////////////////////
// Copyright (c) 2018 Ryan Rounkles                   //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using Plugin.SecureStorage.Abstractions;

namespace Plugin.SecureStorage
{
    /// <summary>
    /// Helper class to facilitate migrating a mac/iOS keychain entry from local-only
    /// storage to icloud (synchronized) storage.
    /// </summary>
    public static class KeychainMigrator
    {
        /// <summary>
        /// Migrates the given key to cloud storage.
        /// </summary>
        /// <returns><c>true</c>, if key was successfully changed to be synchronized <c>false</c> otherwise.</returns>
        /// <param name="key">Key.</param>
        public static bool MigrateToCloudStorage(string key)
        {
            if (null == key)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cloud = new SecureStorageImplementation
            {
                UseCloud = true
            };
            var local = new SecureStorageImplementation
            {
                UseCloud = false
            };

            return Migrate(local, cloud, key);
        }

        /// <summary>
        /// Migrates all the given keys to be synchronized with users iCloud account.
        /// </summary>
        /// <returns><c>true</c>, if all keys migrated to cloud storage successfully, <c>false</c> otherwise.</returns>
        /// <param name="keys">Keys that already exist in keychain</param>
        public static bool MigrateAllToCloudStorage(params string[] keys)
        {
            if(null == keys)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            var cloud = new SecureStorageImplementation
            {
                UseCloud = true
            };
            var local = new SecureStorageImplementation
            {
                UseCloud = false
            };

            var allGood = true;

            foreach (var key in keys)
            {
                if(!Migrate(local, cloud, key))
                {
                    allGood = false;
                }
            }

            return allGood;
        }

        /// <summary>
        /// Migrate a given key, if it exists, to be synchronized.
        /// </summary>
        /// <returns>The migrate.</returns>
        /// <param name="source">Source.</param>
        /// <param name="destination">Destination.</param>
        /// <param name="key">Key.</param>
        static bool Migrate(ISecureStorage source, 
                            ISecureStorage destination, 
                            string key)
        {   
            if(!source.HasKey(key))
            {
                return false;
            }
            var value = source.GetValue(key);
            if(!source.DeleteKey(key))
            {
                return false;
            }
            return destination.SetValue(key, value);
        }
    }
}

////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using Plugin.SecureStorage.Abstractions;

namespace UnitTestSample.Mocks
{
    public class SecureStorageMock : SecureStorageImplementationBase
    {
        /// <summary>
        /// Sets/overrides value for the given key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool SetValue(string key, string value)
        {
            base.SetValue(key, value);

            MockStorage[key] = value;

            return true;
        }

        /// <summary>
        /// Returns value for the given key.
        /// If the key does not exists, returns the value indicated by defaultValue parameter
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public override string GetValue(string key, string defaultValue = null)
        {
            base.GetValue(key, defaultValue);

            if (MockStorage.ContainsKey(key))
            {
                return MockStorage[key];
            }

            return defaultValue;
        }

        /// <summary>
        /// Checks if the key has any value set
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool HasKey(string key)
        {
            base.HasKey(key);

            return MockStorage.ContainsKey(key);
        }

        /// <summary>
        /// Deletes key from the collection, if it exists
        /// Returns true, if the key exists. False, if it does not exist
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool DeleteKey(string key)
        {
            base.DeleteKey(key);

            if (MockStorage.ContainsKey(key))
            {
                MockStorage.Remove(key);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Mock storage to store data
        /// Values in this storage can be used to verify
        /// </summary>
        public Dictionary<string, string> MockStorage { get; } = new Dictionary<string, string>();
    }
}

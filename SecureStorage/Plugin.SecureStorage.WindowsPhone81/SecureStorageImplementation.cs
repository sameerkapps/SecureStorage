using Plugin.SecureStorage.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using System.Runtime.Serialization.Json;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Plugin.SecureStorage
{
    /// <summary>
    /// Not implemented for Windows Phone 8.1
    /// </summary>
    public class SecureStorageImplementation : SecureStorageImplementationBase
    {
        /// <summary>
        /// Not implemented for Windows Phone 8.1
        /// </summary>
        public override bool SetValue(string key, string value)
        {
            throw new NotImplementedException("Not implemented for Windows Phone 8.1");
        }

        /// <summary>
        /// Not implemented for Windows Phone 8.1
        /// </summary>
        public override string GetValue(string key, string defaultValue = null)
        {
            throw new NotImplementedException("Not implemented for Windows Phone 8.1");
        }

        /// <summary>
        /// Not implemented for Windows Phone 8.1
        /// </summary>
        public override bool DeleteKey(string key)
        {
            throw new NotImplementedException("Not implemented for Windows Phone 8.1");
        }

        /// <summary>
        /// Not implemented for Windows Phone 8.1
        /// </summary>
        public override bool HasKey(string key)
        {
            throw new NotImplementedException("Not implemented for Windows Phone 8.1");
        }
    }

}
using Plugin.SecureStorage.Abstractions;
using System;


namespace Plugin.SecureStorage
{
  /// <summary>
  /// Implementation for SecureStorage
  /// </summary>
  public class SecureStorageImplementation : SecureStorageImplementationBase
  {
        /// <summary>
        /// Not implemented for Windows store 8.1
        /// </summary>
        public override bool SetValue(string key, string value)
        {
            throw new NotImplementedException("Not implemented for Windows 8.1");
        }

        /// <summary>
        /// Not implemented for Windows store 8.1
        /// </summary>
        public override string GetValue(string key, string defaultValue = null)
        {
            throw new NotImplementedException("Not implemented for Windows 8.1");
        }

        /// <summary>
        /// Not implemented for Windows store 8.1
        /// </summary>
        public override bool DeleteKey(string key)
        {
            throw new NotImplementedException("Not implemented for Windows 8.1");
        }

        /// <summary>
        /// Not implemented for Windows store 8.1
        /// </summary>
        public override bool HasKey(string key)
        {
            throw new NotImplementedException("Not implemented for Windows 8.1");
        }
    }
}
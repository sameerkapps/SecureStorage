////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;

using Plugin.SecureStorage;
using Plugin.SecureStorage.Abstractions;

namespace SecureStorageSample.PlugInServices
{
    /// <summary>
    /// Provider for plugin
    /// </summary>
    public class PlugInProvider : IPlugInProvider
    {
        /// <summary>
        /// SecureStorage plugin
        /// </summary>
        public ISecureStorage SecureStorage => CrossSecureStorage.Current;
    }
}

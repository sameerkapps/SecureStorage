////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;

using Plugin.SecureStorage.Abstractions;

namespace SecureStorageSample.PlugInServices
{
    /// <summary>
    /// Interface to provide plugin.
    /// This can be mocked in the unit tests
    /// </summary>
    public interface IPlugInProvider
    {
        /// <summary>
        /// Plugin for secure storage
        /// </summary>
        ISecureStorage SecureStorage { get; }
    }
}

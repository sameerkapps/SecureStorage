////////////////////////////////////////////////////////
// Copyright (c) 2018 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;

namespace Plugin.SecureStorage
{
    /// <summary>
    /// StorageTypes for Android
    /// </summary>
    public enum StorageTypes
    {
        /// <summary>
        /// Store in the AndroidKeyStore - Recommended
        /// </summary>
        AndroidKeyStore,

        /// <summary>
        /// Stores in a password protected file.
        /// Provided for backward compatibility
        /// </summary>
        PasswordProtectedFile,
    }
}
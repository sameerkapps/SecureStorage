////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using Plugin.SecureStorage.Abstractions;
using SecureStorageSample.PlugInServices;
using UnitTestSample.Mocks;

namespace SecureStorageSampleNUnitTests.Mocks
{
    /// <summary>
    /// This is a provider for one or more plugins
    /// </summary>
    public class PluginProviderMock : IPlugInProvider
    {
        #region IPlugInProvider implementation
        /// <summary>
        /// Secure storage
        /// </summary>
        public ISecureStorage SecureStorage => _secureStorageMock;
        #endregion

        private SecureStorageMock _secureStorageMock = new SecureStorageMock();
    }
}

using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Security.Cryptography;


namespace Plugin.SecureStorage
{
    /// <summary>
    /// Class that implements persistance of the storage for Windows Phone 8.0 platform
    /// </summary>
    public class SecureStorageImplementation : WinSecureStorageBase
    {

        /// <summary>
        /// Loads the dictionary from storge
        /// </summary>
        protected override byte[] LoadData()
        {
            // get storage for the app
            var storageFile = IsolatedStorageFile.GetUserStoreForApplication();

            // if file exists, open it
            if (storageFile.FileExists(StorageFile))
            {
                using (var stream = storageFile.OpenFile(StorageFile,
                                                          FileMode.Open,
                                                          FileAccess.ReadWrite))
                {
                    // allocate and read the protected data
                    byte[] protectedBytes = new byte[stream.Length];
                    stream.Read(protectedBytes, 0, (int)stream.Length);

                    // obtain clear data by decrypting
                    return ProtectedData.Unprotect(protectedBytes, StoragePasswordArray);
                }
            }

            return null;
        }

        /// <summary>
        /// method to encrypt and save dictionary to storage
        /// </summary>
        protected override void SaveData(byte[] clearBytes)
        {
            // get storage for the app
            var storageFile = IsolatedStorageFile.GetUserStoreForApplication();

            byte[] protectedBytes = ProtectedData.Protect(clearBytes, StoragePasswordArray);

            // save encrypted bytes to file
            using (var stream = storageFile.CreateFile(StorageFile))
            {
                stream.Write(protectedBytes, 0, protectedBytes.Count());
                stream.Close();
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Plugin.SecureStorage
{
    /// <summary>
    /// Implementation of persistence for WinPhone 8.1
    /// Same is applicable for UWP
    /// </summary>
    public class SecureStorageImplementation : WinSecureStorageBase
    {
        /// <summary>
        /// Implementation of Load from storage for Windows Store, WP8.1 and UWP.
        /// </summary>
        protected override byte[] LoadData()
        {
            var task = Task.Run<byte[]>(async () => { return await LoadDataAsync(); });
            task.Wait();

            return task.Result;
        }

        /// <summary>
        /// Synchronous implementation of Save to storage for Windows Store and windows phone 8.1 and UWP
        /// Calls async method and makes it run synchronously
        /// </summary>
        protected override void SaveData(byte[] clearBytes)
        {
            var task = Task.Run(async () => { await SaveDataAsync(clearBytes); });
            task.Wait();
        }

        /// <summary>
        /// Implementation of Load from storage for Windows Store.
        /// </summary>
        protected async Task<byte[]> LoadDataAsync()
        {
            try
            {
                // find the storage file
                var localFolder = ApplicationData.Current.LocalFolder;

                var storageFile = await localFolder.GetFileAsync(StorageFile);

                // read the data. It will be encrypted data
                IBuffer buffProtected = await FileIO.ReadBufferAsync(storageFile);
                DataProtectionProvider provider = new DataProtectionProvider();

                // decrypt the data
                IBuffer clearBuffer = await provider.UnprotectAsync(buffProtected);

                // convert it to byte array
                byte[] clearBytes = new byte[clearBuffer.Length];
                CryptographicBuffer.CopyToByteArray(clearBuffer, out clearBytes);

                return clearBytes;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Implementation of Save to storage for Windows Store, WP8.1 and UWP
        /// </summary>
        private async Task SaveDataAsync(byte[] clearBytes)
        {
            // create the storage file
            var localFolder = ApplicationData.Current.LocalFolder;
            var storageFile = await localFolder.CreateFileAsync(StorageFile, CreationCollisionOption.ReplaceExisting);

            // create buffer from byte array
            IBuffer clearBuffer = CryptographicBuffer.CreateFromByteArray(clearBytes);

            // Encrypt the buffer.
            var provider = new DataProtectionProvider(DPProvider);
            IBuffer protectedBuffer = await provider.ProtectAsync(clearBuffer);

            // save to storage
            await FileIO.WriteBufferAsync(storageFile, protectedBuffer);
        }

        // provider for data protection
        private const string DPProvider = "LOCAL=user";
    }
}
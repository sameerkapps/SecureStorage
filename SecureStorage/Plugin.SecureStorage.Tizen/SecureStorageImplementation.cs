// Thanks to JongHeonChoi for providing the Tizen.net code
// https://github.com/JongHeonChoi/SecureStorage/tree/master/SecureStorage/Plugin.SecureStorage.Tizen

using System;
using System.Text;
using Plugin.SecureStorage.Abstractions;
using Tizen.Security.SecureRepository;

namespace Plugin.SecureStorage
{
    public class SecureStorageImplementation : SecureStorageImplementationBase
    {
        /// <summary>
        /// Password for storage
        /// </summary>
        public static string StoragePassword;

        /// <summary>
        /// Constructor 
        /// </summary>
        public SecureStorageImplementation()
        {
            // verify that password is set
            if (string.IsNullOrWhiteSpace(StoragePassword))
            {
                throw new Exception($"Must set StoragePassword");
            }
        }

        public override string GetValue(string key, string defaultValue)
        {
            base.GetValue(key, defaultValue);

            try
            {
                var storedData = DataManager.Get(key, StoragePassword);
                if (storedData != null)
                {
                    return Encoding.UTF8.GetString(storedData);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return defaultValue;
        }

        public override bool SetValue(string key, string value)
        {
            base.SetValue(key, value);

            try
            {
                byte[] BinaryValue = Encoding.UTF8.GetBytes(value);
                DataManager.Save(key, BinaryValue, new Policy(StoragePassword, true));
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return false;
        }

        public override bool DeleteKey(string key)
        {
            base.DeleteKey(key);

            try
            {
                DataManager.RemoveAlias(key);
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return false;
        }

        public override bool HasKey(string key)
        {
            base.HasKey(key);

            try
            {
                return DataManager.Get(key, StoragePassword) != null;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return false;
        }
    }
}
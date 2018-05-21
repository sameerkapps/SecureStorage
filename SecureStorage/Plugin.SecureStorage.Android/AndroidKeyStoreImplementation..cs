////////////////////////////////////////////////////////
// Copyright (c) 2018 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;
using System.Security.Cryptography;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Security;
using Android.Security.Keystore;

using Java.Security;
using Javax.Crypto;
using Javax.Crypto.Spec;

using Plugin.SecureStorage.Abstractions;

namespace Plugin.SecureStorage
{
    /// <summary>
    /// Implementation for the AndroidKeyStore
    /// </summary>
    public class AndroidKeyStoreImplementation : ISecureStorage
    {
        #region ISecureStorage
        /// <summary>
        /// Gets value from the AndroidKeyStore
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public string GetValue(string key, string defaultValue = null)
        {
            string encStr;
            using (var prefs = AppContext.GetSharedPreferences(SecurePreferenceName, FileCreationMode.Private))
                encStr = prefs.GetString(GetMD5Hash(key), defaultValue);

            string decryptedData = null;
            if (!string.IsNullOrEmpty(encStr))
            {
                var encData = Convert.FromBase64String(encStr);
                var ks = new AndroidKeyStore(AppContext, SecurePreferenceName);
                decryptedData = ks.Decrypt(encData);

                return decryptedData;
            }

            return defaultValue;
        }

        /// <summary>
        /// Sets value in the AndroidKeystore
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(string key, string value)
        {
            try
            {
                var ks = new AndroidKeyStore(AppContext, SecurePreferenceName);
                var encryptedData = ks.Encrypt(value);

                using (var prefs = AppContext.GetSharedPreferences(SecurePreferenceName, FileCreationMode.Private))
                using (var prefsEditor = prefs.Edit())
                {
                    var encStr = Convert.ToBase64String(encryptedData);
                    prefsEditor.PutString(GetMD5Hash(key), encStr);
                    prefsEditor.Commit();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes a value from the AndroidKeyStore
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DeleteKey(string key)
        {
            try
            {
                using (var prefs = AppContext.GetSharedPreferences(SecurePreferenceName, FileCreationMode.Private))
                {
                    var md5Hash = GetMD5Hash(key);
                    if (prefs.Contains(md5Hash))
                    {
                        using (var prefsEditor = prefs.Edit())
                        {
                            prefsEditor.Remove(md5Hash);
                            prefsEditor.Commit();

                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Checks, if it has the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasKey(string key)
        {
            try
            {
                using (var prefs = AppContext.GetSharedPreferences(SecurePreferenceName, FileCreationMode.Private))
                {
                    return prefs.Contains(GetMD5Hash(key));
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Generates MD5 hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GetMD5Hash(string input)
        {
            var hash = new StringBuilder();
            var md5provider = new MD5CryptoServiceProvider();
            var bytes = md5provider.ComputeHash(Encoding.UTF8.GetBytes(input));

            for (var i = 0; i < bytes.Length; i++)
                hash.Append(bytes[i].ToString("x2"));

            return hash.ToString();
        }
        #endregion

        #region AndroidKeyStore
        /// <summary>
        /// This class is taken from Xamarin Essentials
        /// </summary>
        private class AndroidKeyStore
        {
            const string androidKeyStore = "AndroidKeyStore"; // this is an Android const value
            const string aesAlgorithm = "AES";
            const string cipherTransformationAsymmetric = "RSA/ECB/PKCS1Padding";
            const string cipherTransformationSymmetric = "AES/GCM/NoPadding";
            const string prefsMasterKey = "SecureStorageKey";
            const int initializationVectorLen = 12; // Android supports an IV of 12 for AES/GCM

            internal AndroidKeyStore(Context context, string keystoreAlias)
            {
                appContext = context;
                alias = keystoreAlias;

                keyStore = KeyStore.GetInstance(androidKeyStore);
                keyStore.Load(null);
            }

            Context appContext;
            string alias;
            KeyStore keyStore;

            ISecretKey GetKey()
            {
                // If >= API 23 we can use the KeyStore's symmetric key
                if (HasApiLevel(BuildVersionCodes.M))
                    return GetSymmetricKey();

                // NOTE: KeyStore in < API 23 can only store asymmetric keys
                // specifically, only RSA/ECB/PKCS1Padding
                // So we will wrap our symmetric AES key we just generated
                // with this and save the encrypted/wrapped key out to
                // preferences for future use.
                // ECB should be fine in this case as the AES key should be
                // contained in one block.

                // Get the asymmetric key pair
                var keyPair = GetAsymmetricKeyPair();

                using (var prefs = appContext.GetSharedPreferences(alias, FileCreationMode.Private))
                {
                    var existingKeyStr = prefs.GetString(prefsMasterKey, null);

                    if (!string.IsNullOrEmpty(existingKeyStr))
                    {
                        var wrappedKey = Convert.FromBase64String(existingKeyStr);

                        var unwrappedKey = UnwrapKey(wrappedKey, keyPair.Private);
                        var kp = unwrappedKey.JavaCast<ISecretKey>();

                        return kp;
                    }
                    else
                    {
                        var keyGenerator = KeyGenerator.GetInstance(aesAlgorithm);
                        var defSymmetricKey = keyGenerator.GenerateKey();

                        var wrappedKey = WrapKey(defSymmetricKey, keyPair.Public);

                        using (var prefsEditor = prefs.Edit())
                        {
                            prefsEditor.PutString(prefsMasterKey, Convert.ToBase64String(wrappedKey));
                            prefsEditor.Commit();
                        }

                        return defSymmetricKey;
                    }
                }
            }

            // API 23+ Only
            ISecretKey GetSymmetricKey()
            {
                var existingKey = keyStore.GetKey(alias, null);

                if (existingKey != null)
                {
                    var existingSecretKey = existingKey.JavaCast<ISecretKey>();
                    return existingSecretKey;
                }

                var keyGenerator = KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmAes, androidKeyStore);
                var builder = new KeyGenParameterSpec.Builder(alias, KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
                    .SetBlockModes(KeyProperties.BlockModeGcm)
                    .SetEncryptionPaddings(KeyProperties.EncryptionPaddingNone)
                    .SetRandomizedEncryptionRequired(false);

                keyGenerator.Init(builder.Build());

                return keyGenerator.GenerateKey();
            }

            KeyPair GetAsymmetricKeyPair()
            {
                var asymmetricAlias = $"{alias}.asymmetric";

                var privateKey = keyStore.GetKey(asymmetricAlias, null)?.JavaCast<IPrivateKey>();
                var publicKey = keyStore.GetCertificate(asymmetricAlias)?.PublicKey;

                // Return the existing key if found
                if (privateKey != null && publicKey != null)
                    return new KeyPair(publicKey, privateKey);

                // Otherwise we create a new key
                var generator = KeyPairGenerator.GetInstance(KeyProperties.KeyAlgorithmRsa, androidKeyStore);

                var end = DateTime.UtcNow.AddYears(20);
                var startDate = new Java.Util.Date();
                var endDate = new Java.Util.Date(end.Year, end.Month, end.Day);

#pragma warning disable CS0618
                var builder = new KeyPairGeneratorSpec.Builder(Application.Context)
                    .SetAlias(asymmetricAlias)
                    .SetSerialNumber(Java.Math.BigInteger.One)
                    .SetSubject(new Javax.Security.Auth.X500.X500Principal($"CN={asymmetricAlias} CA Certificate"))
                    .SetStartDate(startDate)
                    .SetEndDate(endDate);

                generator.Initialize(builder.Build());
#pragma warning restore CS0618

                return generator.GenerateKeyPair();
            }

            byte[] WrapKey(IKey keyToWrap, IKey withKey)
            {
                var cipher = Cipher.GetInstance(cipherTransformationAsymmetric);
                cipher.Init(Javax.Crypto.CipherMode.WrapMode, withKey);
                return cipher.Wrap(keyToWrap);
            }

            IKey UnwrapKey(byte[] wrappedData, IKey withKey)
            {
                var cipher = Cipher.GetInstance(cipherTransformationAsymmetric);
                cipher.Init(Javax.Crypto.CipherMode.UnwrapMode, withKey);
                var unwrapped = cipher.Unwrap(wrappedData, KeyProperties.KeyAlgorithmAes, KeyType.SecretKey);
                return unwrapped;
            }

            internal byte[] Encrypt(string data)
            {
                var key = GetKey();

                // Generate initialization vector
                var iv = new byte[initializationVectorLen];
                var sr = new SecureRandom();
                sr.NextBytes(iv);

                Cipher cipher;

                // Attempt to use GCMParameterSpec by default
                try
                {
                    cipher = Cipher.GetInstance(cipherTransformationSymmetric);
                    cipher.Init(Javax.Crypto.CipherMode.EncryptMode, key, new GCMParameterSpec(128, iv));
                }
                catch (Java.Security.InvalidAlgorithmParameterException)
                {
                    // If we encounter this error, it's likely an old bouncycastle provider version
                    // is being used which does not recognize GCMParameterSpec, but should work
                    // with IvParameterSpec, however we only do this as a last effort since other
                    // implementations will error if you use IvParameterSpec when GCMParameterSpec
                    // is recognized and expected.
                    cipher = Cipher.GetInstance(cipherTransformationSymmetric);
                    cipher.Init(Javax.Crypto.CipherMode.EncryptMode, key, new IvParameterSpec(iv));
                }

                var decryptedData = Encoding.UTF8.GetBytes(data);
                var encryptedBytes = cipher.DoFinal(decryptedData);

                // Combine the IV and the encrypted data into one array
                var r = new byte[iv.Length + encryptedBytes.Length];
                Buffer.BlockCopy(iv, 0, r, 0, iv.Length);
                Buffer.BlockCopy(encryptedBytes, 0, r, iv.Length, encryptedBytes.Length);

                return r;
            }

            internal string Decrypt(byte[] data)
            {
                if (data.Length < initializationVectorLen)
                    return null;

                var key = GetKey();

                // IV will be the first 16 bytes of the encrypted data
                var iv = new byte[initializationVectorLen];
                Buffer.BlockCopy(data, 0, iv, 0, initializationVectorLen);

                Cipher cipher;

                // Attempt to use GCMParameterSpec by default
                try
                {
                    cipher = Cipher.GetInstance(cipherTransformationSymmetric);
                    cipher.Init(Javax.Crypto.CipherMode.DecryptMode, key, new GCMParameterSpec(128, iv));
                }
                catch (Java.Security.InvalidAlgorithmParameterException)
                {
                    // If we encounter this error, it's likely an old bouncycastle provider version
                    // is being used which does not recognize GCMParameterSpec, but should work
                    // with IvParameterSpec, however we only do this as a last effort since other
                    // implementations will error if you use IvParameterSpec when GCMParameterSpec
                    // is recognized and expected.
                    cipher = Cipher.GetInstance(cipherTransformationSymmetric);
                    cipher.Init(Javax.Crypto.CipherMode.DecryptMode, key, new IvParameterSpec(iv));
                }

                // Decrypt starting after the first 16 bytes from the IV
                var decryptedData = cipher.DoFinal(data, initializationVectorLen, data.Length - initializationVectorLen);

                return Encoding.UTF8.GetString(decryptedData);
            }

            private bool HasApiLevel(BuildVersionCodes versionCode) => (int)Build.VERSION.SdkInt >= (int)versionCode;
        }
        #endregion

        #region fields
        private readonly string PackageName = Application.Context.PackageName;

        private readonly Context AppContext = Application.Context;

        private string SecurePreferenceName => $"{PackageName}.SecureStorage";
        #endregion
    }
}
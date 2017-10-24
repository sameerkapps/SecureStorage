using System;
using Plugin.SecureStorage.Abstractions;

namespace Plugin.SecureStorage
{
    /// <summary>
    /// Cross platform SecureStorage implemenations
    /// </summary>
    public class CrossSecureStorage
    {
        static Lazy<ISecureStorage> Implementation = new Lazy<ISecureStorage>(() => CreateSecureStorage(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Gets whether the plugin is supported on the current platform
        /// </summary>
        public static bool IsSupported => Implementation.Value != null;

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static ISecureStorage Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static ISecureStorage CreateSecureStorage()
        {
#if NETSTANDARD1_0
        return null;
#else
            return new SecureStorageImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}

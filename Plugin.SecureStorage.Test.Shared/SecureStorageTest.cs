using Plugin.SecureStorage.Abstractions;
using Plugin.SecureStorage.Test;

namespace Plugin.SecureStorage.Net46.Test
{
    public class SecureStorageTest : TestBase
    {
        protected override ISecureStorage GetTarget()
        {
            return CrossSecureStorage.Current;
        }
    }
}

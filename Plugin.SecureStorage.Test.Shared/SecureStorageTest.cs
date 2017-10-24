using Plugin.SecureStorage.Abstractions;

namespace Plugin.SecureStorage.Test
{
    public class SecureStorageTest : TestBase
    {
        protected override ISecureStorage GetTarget()
        {
            return CrossSecureStorage.Current;
        }
    }
}

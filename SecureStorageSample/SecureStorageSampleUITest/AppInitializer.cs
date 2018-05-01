using System;
using System.IO;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace SecureStorageSampleUITest
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp
                    .Android
                    .ApkFile("../../../SecureStorageSample/SecureStorageSample.Android/bin/Debug/com.companyname.SecureStorageSample.apk")
                    .StartApp();
            }

            // for iOS add reference to the project and
            // adjusted this as per your settings for iOS
            const string simId = "1B7659DC-8BB3-4F26-A84E-7D2B8321F7DB"; //iPhone 5s (11.3 Simulator)
            return ConfigureApp
                .iOS
                .AppBundle("../../../SecureStorageSample/SecureStorageSample.iOS/bin/iPhoneSimulator/Debug/device-builds/iphone6.1-11.3/SecureStorageSample.iOS.app")
                .DeviceIdentifier(simId)
                .StartApp();
        }
    }
}


using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.SecureStorage;

namespace SecureStorageSample.Droid
{
    [Activity(Label = "SecureStorageSample", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            // The new default storage type for android is AndroidKeyStore.
            // AndroidKeyStore provides the mechanism to store its own credentials that only that app can use.
            // ref: https://developer.android.com/training/articles/keystore

            // If you want backward compatibility and/or do not wish to store secure data in the AndroidKeyStore,
            // set the storage type to password protected file. This will use "Build.Serial" as its password.
            // SecureStorageImplementation.StorageType = StorageTypes.PasswordProtectedFile;

            // If you want a different password for password protected file, set it as follows
            // obfuscate it to prevent it from being discovered by reverse engineering.
            // ProtectedFileImplementation.StoragePassword = "YourPassword";

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}


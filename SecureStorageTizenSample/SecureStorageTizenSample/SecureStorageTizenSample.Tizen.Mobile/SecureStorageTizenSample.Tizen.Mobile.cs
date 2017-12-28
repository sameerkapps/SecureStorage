using Plugin.SecureStorage;
using System;

namespace SecureStorageTizenSample.Tizen.Mobile
{
    class Program : global::Xamarin.Forms.Platform.Tizen.FormsApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            LoadApplication(new App());
        }

        static void Main(string[] args)
        {
            var app = new Program();
            // You can choose your own password and obfuscate the app.
            SecureStorageImplementation.StoragePassword = "YOUR PASSWORD";
            global::Xamarin.Forms.Platform.Tizen.Forms.Init(app);
            app.Run(args);
        }
    }
}

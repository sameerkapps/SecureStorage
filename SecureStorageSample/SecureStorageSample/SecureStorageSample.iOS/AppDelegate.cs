using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Plugin.SecureStorage;
using UIKit;

namespace SecureStorageSample.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            // Newer version of Visual Studio for Mac and Visual Studio provide the
            // ENABLE_TEST_CLOUD compiler directive in the Debug configuration,
            // but not the Release configuration.
#if ENABLE_TEST_CLOUD
            Xamarin.Calabash.Start();
#endif
            // Set the keychain accessibility if desired. Default is AfterFirstUnlock
            // The keys stored using earlier packages (<= 2.0.1) will not be accessible with the new default
            // To use the earlier keys, set it to Invalid :(
            // SecureStorageImplementation.DefaultAccessible = Security.SecAccessible.Invalid;

            return base.FinishedLaunching(app, options);
        }
    }
}

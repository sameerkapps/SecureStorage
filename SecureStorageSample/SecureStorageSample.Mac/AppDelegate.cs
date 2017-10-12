using AppKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;
//using Plugin.SecureStorage;


namespace SecureStorageSample.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        NSWindow mainWindow;

        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

            var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
            mainWindow = new NSWindow(rect, style, NSBackingStore.Buffered, false);
            mainWindow.Title = "SecureStorageSample";
            mainWindow.TitleVisibility = NSWindowTitleVisibility.Hidden;
        }

        public override NSWindow MainWindow
        {
            get
            {
                return mainWindow;
            }
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            Forms.Init();
            LoadApplication(new App());
            base.DidFinishLaunching(notification);
        }


        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }

}

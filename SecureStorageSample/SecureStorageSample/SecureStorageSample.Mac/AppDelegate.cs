// ////////////////////////////////////////////////////////////
// // Copyright 2017 Sameer Khandekar                        //
// // License: MIT License                                   //
// ////////////////////////////////////////////////////////////
using AppKit;
using Foundation;

using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace SecureStorageSample.Mac
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

            var rect = new CoreGraphics.CGRect(200, 1000, 1024, 768);
            _window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
            _window.Title = "Xamarin.Forms on Mac!";
            _window.TitleVisibility = NSWindowTitleVisibility.Hidden;                
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
            Forms.Init();
            LoadApplication(new App());
            base.DidFinishLaunching(notification);
        }

        public override NSWindow MainWindow
        {
            get { return _window; }
        }

        NSWindow _window;
    }
}

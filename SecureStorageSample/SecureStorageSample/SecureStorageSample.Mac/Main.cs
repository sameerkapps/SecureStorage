// ////////////////////////////////////////////////////////////
// // Copyright 2017 Sameer Khandekar                        //
// // License: MIT License                                   //
// ////////////////////////////////////////////////////////////
using AppKit;

namespace SecureStorageSample.Mac
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            NSApplication.Init();
            NSApplication.SharedApplication.Delegate = new AppDelegate();
            NSApplication.Main(args);
        }
    }
}

using System;
using System.Diagnostics;

using Xamarin.Forms;
using SecureStorageSample.ViewModels;
using SecureStorageSample.PlugInServices;


namespace SecureStorageSample
{
	public partial class App : Application
	{
        public App()
        {
            InitializeComponent();
            RegisterImplementations();
            var vm = new MainPageViewModel();
            MainPage = new SecureStorageSample.MainPage() { BindingContext = vm };
		}

        protected override void OnStart ()
		{
            // Handle when your app starts

            // get the secure storage
            var secureStorage = DependencyService.Get<IPlugInProvider>().SecureStorage;

            // set value
            secureStorage.SetValue(TestOnStartKey, "123");

            // retrieve it and trace it
            var ret = secureStorage.GetValue(TestOnStartKey);
            Trace.WriteLine($"OnStart ret = {ret}");

            // clenanup
            secureStorage.DeleteKey(TestOnStartKey);
        }

		protected override void OnSleep ()
		{
            // Handle when your app sleeps
            // get the secure storage
            var secureStorage = DependencyService.Get<IPlugInProvider>().SecureStorage;
            // set value
            secureStorage.SetValue(TestSleepResumeKey, "456");

            // retrieve it and trace it
            var ret = secureStorage.GetValue(TestSleepResumeKey);
            Trace.WriteLine($"OnSleep ret = {ret}");
        }

		protected override void OnResume ()
		{
            // Handle when your app resumes
            // get the secure storage
            var secureStorage = DependencyService.Get<IPlugInProvider>().SecureStorage;

            // retrieve it and trace it
            var ret = secureStorage.GetValue(TestSleepResumeKey);
            Trace.WriteLine($"OnResume ret = {ret}");

            // cleanup
            secureStorage.DeleteKey(TestSleepResumeKey);
        }

        private void RegisterImplementations()
        {
            DependencyService.Register<IPlugInProvider, PlugInProvider>();
        }

        #region constants
        // Key for testing onstart
        private const string TestOnStartKey = "OnStartKey";

        // key for testing resume, sleep
        private const string TestSleepResumeKey = "OnSleepResumeKey";
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using SecureStorageSample.ViewModels;

namespace SecureStorageSample
{
	public partial class App : Application
	{
        public App()
        {
            InitializeComponent();
            var vm = new MainPageViewModel();
            MainPage = new SecureStorageSample.MainPage() { BindingContext = vm };
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

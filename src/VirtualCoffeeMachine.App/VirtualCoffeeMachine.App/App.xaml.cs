using System;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace VirtualCoffeeMachine.App
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
			SimpleIoc.Default.Register<ViewModels.Services.IMessage, Services.Message>();
			SimpleIoc.Default.Register<ViewModels.Services.INavigation, Services.Navigation>();
			SimpleIoc.Default.Register(() => new ViewModels.CoffeeMachine());

			MainPage = new MainPage();
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

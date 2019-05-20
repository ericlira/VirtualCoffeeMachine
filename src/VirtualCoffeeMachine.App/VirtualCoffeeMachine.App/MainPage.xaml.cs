using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms;

namespace VirtualCoffeeMachine.App
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
			BindingContext = SimpleIoc.Default.GetInstance<ViewModels.CoffeeMachine>();
		}
	}
}

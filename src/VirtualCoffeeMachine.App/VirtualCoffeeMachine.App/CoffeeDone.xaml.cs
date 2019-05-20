using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VirtualCoffeeMachine.App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CoffeeDone : ContentPage
	{
		public CoffeeDone ()
		{
			InitializeComponent ();
			BindingContext = SimpleIoc.Default.GetInstance<ViewModels.CoffeeMachine>();
		}
	}
}
namespace VirtualCoffeeMachine.App.Services
{
	public class Navigation : ViewModels.Services.INavigation
	{
		public void NavigateToMain()
		{
			App.Current.MainPage = new MainPage();
		}

		public void NavigateToAddCoins()
		{
			App.Current.MainPage = new AddCoins();
		}

		public void NavigateToCoffeeDone()
		{
			App.Current.MainPage = new CoffeeDone();
		}

		public void NavigateToCancelOrder()
		{
			App.Current.MainPage = new CoffeeDone();
		}
	}
}

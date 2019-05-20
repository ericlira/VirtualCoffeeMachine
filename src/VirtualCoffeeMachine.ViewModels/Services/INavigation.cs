using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCoffeeMachine.ViewModels.Services
{
	public interface INavigation
	{
		void NavigateToMain();
		void NavigateToAddCoins();
		void NavigateToCoffeeDone();
		void NavigateToCancelOrder();
	}
}

using System;
using System.Threading.Tasks;

namespace VirtualCoffeeMachine.ViewModels.Services
{
	public interface IMessage
	{
		Task ShowSuccess(string message);

		Task ShowAlert(string message);

		Task ShowError(Exception error);

		Task<bool> GetConfirmationAsync(string message);

		void Toast(string message);

		void HideLoading();

		void ShowLoading(string message = null);
	}
}

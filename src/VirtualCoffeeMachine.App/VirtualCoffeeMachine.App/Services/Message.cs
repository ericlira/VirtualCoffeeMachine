using System;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace VirtualCoffeeMachine.App.Services
{
	public class Message : ViewModels.Services.IMessage
	{
		#region Metodos
		public async void HideLoading()
		{
			await Task.Delay(5);
			UserDialogs.Instance.HideLoading();
		}

		public void ShowLoading(string message = null)
		{
			UserDialogs.Instance.ShowLoading(message);
		}

		public async Task ShowAlert(string message)
		{
			await UserDialogs.Instance.AlertAsync(message, "Atention!", "OK");
		}

		public async Task ShowError(Exception error)
		{
			string message = error.InnerException != null ? error.InnerException.Message : error.Message;
			await ShowAlert(message);
		}

		public async Task ShowSuccess(string message)
		{
			await UserDialogs.Instance.AlertAsync(message, "Information!", "OK");
		}

		public async Task<bool> GetConfirmationAsync(string message)
		{
			var config = new ConfirmConfig()
			{
				Title = "Confirmation",
				Message = message,
				OkText = "Yes",
				CancelText = "No",
			};
			return await UserDialogs.Instance.ConfirmAsync(config);
		}

		public void Toast(string message)
		{
			UserDialogs.Instance.Toast(message);
		}
		#endregion
	}
}

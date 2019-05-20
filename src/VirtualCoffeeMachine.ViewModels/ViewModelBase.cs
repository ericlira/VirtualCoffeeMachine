using GalaSoft.MvvmLight.Ioc;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VirtualCoffeeMachine.ViewModels
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		#region Atributos
		//protected readonly Services.IConfiguration configuration;
		protected readonly Services.IMessage Message;
		protected readonly Services.INavigation Navigation;
		//protected readonly Services.IDeviceID deviceID;

		private bool isBusy;
		#endregion

		#region Propriedades
		public bool IsBusy
		{
			get { return isBusy; }
			set
			{
				SetProperty(ref isBusy, value);

				ShowHideLoading();

				RaiseCanExecuteChanged();
			}
		}
		#endregion

		#region Construtor
		public ViewModelBase()
		{
			//configuration = SimpleIoc.Default.GetInstance<Services.IConfiguration>();
			Message = SimpleIoc.Default.GetInstance<Services.IMessage>();
			Navigation = SimpleIoc.Default.GetInstance<Services.INavigation>();
			//deviceID = SimpleIoc.Default.GetInstance<Services.IDeviceID>();
			IsBusy = false;
		}
		#endregion

		#region Metodos
		private void ShowHideLoading()
		{
			if (IsBusy)
				Message.ShowLoading();
			else
				Message.HideLoading();
		}

		protected virtual void RaiseCanExecuteChanged()
		{
		}
		#endregion

		#region Interface
		public event PropertyChangedEventHandler PropertyChanged;
		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(storage, value))
			{
				return false;
			}

			storage = value;
			OnPropertyChanged(propertyName);
			return true;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}

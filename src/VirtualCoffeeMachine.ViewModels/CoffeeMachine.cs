using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace VirtualCoffeeMachine.ViewModels
{
	public class CoffeeMachine : ViewModelBase
	{
		#region Backing fields
		private ObservableCollection<Models.Coffee> coffees;
		private ObservableCollection<Models.Coin> register;
		private ObservableCollection<Models.Coin> payment;
		private decimal totalPayment;
		private ObservableCollection<Models.Coin> change;
		private Models.Coffee selectedCoffee;
		#endregion

		#region Properties
		public ObservableCollection<Models.Coffee> Coffees
		{
			get { return coffees; }
			set { SetProperty(ref coffees, value); }
		}

		public ObservableCollection<Models.Coin> Register
		{
			get { return register; }
			set { SetProperty(ref register, value); }
		}

		public ObservableCollection<Models.Coin> Payment
		{
			get
			{
				if (payment == null)
				{
					Payment = new ObservableCollection<Models.Coin>();
				}
				return payment;
			}
			set { SetProperty(ref payment, value); }
		}

		public ObservableCollection<Models.Coin> Change
		{
			get
			{
				if (change == null)
				{
					Change = new ObservableCollection<Models.Coin>();
				}
				return change;
			}
			set { SetProperty(ref change, value); }
		}

		public Models.Coffee SelectedCoffee
		{
			get { return selectedCoffee; }
			set { SetProperty(ref selectedCoffee, value); }
		}

		public bool IsCoffeeSelected
		{
			get
			{
				return SelectedCoffee != null;
			}
		}

		public decimal TotalPayment
		{
			get { return totalPayment; }
			set { SetProperty(ref totalPayment, value); }
		}

		public decimal TotalChange
		{
			get
			{
				return Change.Sum(s => s.Value * s.QuantityAvailable);
			}
		}

		public bool HasChange
		{
			get
			{
				return Change.Count > 0;
			}
		}
		#endregion

		#region Constructor
		public CoffeeMachine()
		{
			StartupMachine();

			ConfigureCommands();

			Payment.CollectionChanged += Payment_CollectionChanged;
		}
		#endregion

		#region Events
		private void Payment_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			ComputeTotalPayment();
		}
		#endregion

		#region Commands
		public ICommand NavigateToAddCoinCommand { get; set; }
		public ICommand NavigateToMainCommand { get; set; }
		public ICommand AddCoinCommand { get; set; }
		public ICommand ConfirmOrderCommand { get; set; }
		public ICommand CancelOrderCommand { get; set; }
		public ICommand NewOrderCommand { get; set; }
		#endregion

		#region Methods
		private void StartupMachine()
		{
			//
			// Use this method to configure the startup coffees and coins with each respectively quantity
			//

			// Adding types of coffee and quantity available of each.
			Coffees = new ObservableCollection<Models.Coffee>()
			{
				new Models.Coffee()
				{
					Number = 1,
					Name = "Cappuccino",
					Description = "ESPRESSO AND EQUAL PARTS STEAMED MILK AND FOAM",
					ImageSource = "cappuccino.png",
					Price = 3.5M,
					QuantityAvailable = 5
				},
				new Models.Coffee()
				{
					Number = 2,
					Name = "Latte",
					Description = "ESPRESSO AND STEAMED MILK TOPPED WITH CREAMY FOAM",
					ImageSource = "latte.png",
					Price = 3M,
					QuantityAvailable = 10
				},
				new Models.Coffee()
				{
					Number = 3,
					Name = "Decaf",
					Description = "OSMOSIS-DECAFFEINATED (WATER PROCESS) COFFEE",
					ImageSource = "decaf.png",
					Price = 4M,
					QuantityAvailable = 10
				}
			};

			// Adding acceptable coins and quantity available for change.
			Register = new ObservableCollection<Models.Coin>()
			{
				new Models.Coin() { Value = 0.01M, Name = "1 cent", QuantityAvailable = 10, Acceptable = false },
				new Models.Coin() { Value = 0.02M, Name = "2 cents", QuantityAvailable = 10, Acceptable = false },
				new Models.Coin() { Value = 0.05M, Name = "5 cents", QuantityAvailable = 10, Acceptable = true },
				new Models.Coin() { Value = 0.10M, Name = "10 cents", QuantityAvailable = 10, Acceptable = true },
				new Models.Coin() { Value = 0.20M, Name = "20 cents", QuantityAvailable = 10, Acceptable = true },
				new Models.Coin() { Value = 0.50M, Name = "50 cents", QuantityAvailable = 10, Acceptable = true },
				new Models.Coin() { Value = 1M, Name = "1 dollar", QuantityAvailable = 5, Acceptable = true },
				new Models.Coin() { Value = 2M, Name = "2 dollars", QuantityAvailable = 5, Acceptable = true },
			};
		}

		private void ConfigureCommands()
		{
			NavigateToAddCoinCommand = new RelayCommand(execute: () => Navigation.NavigateToAddCoins());
			NavigateToMainCommand = new RelayCommand(execute: () => Navigation.NavigateToMain());
			AddCoinCommand = new RelayCommand<Models.Coin>(execute: (coin) => AddCoin(coin));
			ConfirmOrderCommand = new RelayCommand(execute: async () => await ConfirmOrder());
			CancelOrderCommand = new RelayCommand(execute: CancelOrder, canExecute: () => TotalPayment > 0);
			NewOrderCommand = new RelayCommand(execute: NewOrder);
		}

		private void AddCoin(Models.Coin coin)
		{
			// Check if coin is acceptable.
			if (Register.FirstOrDefault(p => p.Value == coin.Value && p.Acceptable) != null)
			{
				Payment.Add(coin.Copy());
			}
			else
			{
				Message.ShowAlert($"Sorry, {coin.Name} coins are not acceptable at this moment.");
			}
		}

		private void ComputeTotalPayment()
		{
			TotalPayment = Payment.Sum(s => s.Value);
			RaiseCanExecuteChanged();
		}

		private void AcceptPayment()
		{
			// Send payment to register.
			foreach (var group in Payment.GroupBy(k => k.Value))
			{
				if (Register.FirstOrDefault(p => p.Value == group.Key && p.Acceptable) is Models.Coin registerCoin)
				{
					registerCoin.QuantityAvailable += group.Count();
				}
			}
		}

		private async Task ConfirmOrder()
		{
			if (SelectedCoffee == null)
			{
				// No coffee selected.
				await Message.ShowAlert("Please select your coffee!");
				return;
			}
			else if (Coffees.FirstOrDefault(p => p.Number == SelectedCoffee.Number && p.Available) == null)
			{
				// Not enough coffee.
				var possibleCoffee = Coffees.FirstOrDefault(p => p.Available);
				string messagePossibleCoffee = possibleCoffee != null ? $"{Environment.NewLine}How about a {possibleCoffee.Name} instead?" : string.Empty;
				await Message.ShowAlert($"Sorry, out of {SelectedCoffee.Name}!{messagePossibleCoffee}");
				return;
			}
			else if (TotalPayment < SelectedCoffee.Price)
			{
				// Not enough money.
				var possibleCoffee = Coffees.FirstOrDefault(p => p.Price == TotalPayment && p.Available);
				string messagePossibleCoffee = possibleCoffee != null ? $" or you could have a {possibleCoffee.Name}" : string.Empty;
				await Message.ShowAlert($"Sorry, not enough money for a {SelectedCoffee.Name}!{Environment.NewLine}Please add more coins{messagePossibleCoffee}!");
				return;
			}

			try
			{
				AcceptPayment();

				// MakeChange can't get necessary change, ask user to continue or not.
				if (MakeChange() || await Message.GetConfirmationAsync($"Not enough change! Would you like to confirm your order anyway? I'm missing ${(TotalPayment - SelectedCoffee.Price - TotalChange):N2}."))
				{
					await MakeCoffee();
					Navigation.NavigateToCoffeeDone();
				}
			}
			catch
			{
				// If something goes wrong, give money back.
				CancelOrder();
			}
		}

		private void CancelOrder()
		{
			Change.Clear();

			// Give back cois as it was inputed
			foreach (var group in Payment.GroupBy(k => k.Value))
			{
				var paymentCoin = Payment.FirstOrDefault(p => p.Value == group.Key);
				var coin = paymentCoin.Copy();
				coin.QuantityAvailable = group.Count();
				Change.Add(coin);
			}

			Payment.Clear();

			SelectedCoffee = null;

			Message.Toast("See you next time!");

			Navigation.NavigateToCancelOrder();
		}

		private bool MakeChange()
		{
			//
			// Change is given using a greedy algorithm
			//

			var changeToGive = TotalPayment - SelectedCoffee.Price;

			while (changeToGive > 0)
			{
				if (Register.OrderByDescending(k => k.Value).FirstOrDefault(p => p.Value <= changeToGive && p.QuantityAvailable > 0) is Models.Coin registerCoin)
				{
					var changeCoin = Change.FirstOrDefault(p => p.Value == registerCoin.Value);
					if (changeCoin == null)
					{
						changeCoin = new Models.Coin() { Name = registerCoin.Name, Value = registerCoin.Value, QuantityAvailable = 1 };
						Change.Add(changeCoin);
					}
					else
					{
						changeCoin.QuantityAvailable++;
					}

					registerCoin.QuantityAvailable--;
					changeToGive -= changeCoin.Value;
				}
				else
				{
					break;
				}
			}

			if (Change.Count > 0)
			{
				Message.Toast("Don't forget to take your change.");
			}

			return changeToGive == 0;
		}

		private async Task MakeCoffee()
		{
			try
			{
				Message.ShowLoading("Just a moment while I make your coffee...");

				// Updates available quantity of selected coffee
				var coffee = Coffees.FirstOrDefault(p => p.Number == SelectedCoffee.Number);
				coffee.QuantityAvailable--;

				await Task.Delay(5000);
			}
			finally
			{
				Message.HideLoading();
			}
		}

		private void NewOrder()
		{
			Payment.Clear();
			Change.Clear();
			SelectedCoffee = null;
			Navigation.NavigateToMain();
		}

		protected override void RaiseCanExecuteChanged()
		{
			((RelayCommand)CancelOrderCommand)?.RaiseCanExecuteChanged();
		}
		#endregion
	}
}

namespace VirtualCoffeeMachine.Models
{
	public class Coin : ModelBase
	{
		#region Backing fields
		private decimal _value;
		private string name;
		private int quantityAvailable;
		private bool acceptable;
		#endregion

		#region Properties
		public decimal Value
		{
			get { return _value; }
			set { SetProperty(ref _value, value); }
		}

		public string Name
		{
			get { return name; }
			set { SetProperty(ref name, value); }
		}

		public int QuantityAvailable
		{
			get { return quantityAvailable; }
			set { SetProperty(ref quantityAvailable, value); }
		}

		public bool Available
		{
			get
			{
				return QuantityAvailable > 0;
			}
		}

		public bool Acceptable
		{
			get { return acceptable; }
			set { SetProperty(ref acceptable, value); }
		}
		#endregion

		#region Methods
		public Coin Copy()
		{
			return new Coin() { Name = Name, Value = Value, QuantityAvailable = 1 };
		}
		#endregion
	}
}

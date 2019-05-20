using System;

namespace VirtualCoffeeMachine.Models
{
	public class Coffee : ModelBase
	{
		#region Backing fields
		private int number;
		private string imageSource;
		private string name;
		private string description;
		private decimal price;
		private int quantityAvailable;
		#endregion

		#region Properties
		public int Number
		{
			get { return number; }
			set { SetProperty(ref number, value); }
		}

		public string ImageSource
		{
			get { return imageSource; }
			set { SetProperty(ref imageSource, value); }
		}

		public string Name
		{
			get { return name; }
			set { SetProperty(ref name, value); }
		}

		public string Description
		{
			get { return description; }
			set { SetProperty(ref description, value); }
		}

		public decimal Price
		{
			get { return price; }
			set { SetProperty(ref price, value); }
		}

		public string DisplayPrice
		{
			get
			{
				return $"${Price.ToString("N2")}";
			}
		}

		public int QuantityAvailable
		{
			get { return quantityAvailable; ; }
			set { quantityAvailable = value; }
		}

		public bool Available
		{
			get
			{
				return QuantityAvailable > 0;
			}
		}
		#endregion
	}
}

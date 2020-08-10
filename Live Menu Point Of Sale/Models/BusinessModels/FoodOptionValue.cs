using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.Models.BusinessModels
{
	[Serializable]
    public class FoodOptionValue : PropertyChangedBase
    {
		public FoodOptionValue()
		{
			Id = Guid.NewGuid();
		}

		private Guid _id;

		public Guid Id
		{
			get { return _id; }
			set { _id = value; NotifyOfPropertyChange(() => Id); }
		}

		private string _name;

		public string Name
		{
			get { return _name; }
			set { _name = value; NotifyOfPropertyChange(() => Name); }
		}

		
		private double _deliveryPrice;

		public double DeliveryPrice
		{
			get { return _deliveryPrice; }
			set { _deliveryPrice = value; NotifyOfPropertyChange(() => DeliveryPrice); }
		}

		private double _dineInPrice;

		public double DineInPrice
		{
			get { return _dineInPrice; }
			set { _dineInPrice = value; NotifyOfPropertyChange(() => DineInPrice); }
		}

		private double _collectionPrice;

		public double CollectionPrice
		{
			get { return _collectionPrice; }
			set { _collectionPrice = value; NotifyOfPropertyChange(() => CollectionPrice); }
		}

		private Guid _optionKeyId;

		public Guid OptionKeyId
		{
			get { return _optionKeyId; }
			set { _optionKeyId = value; NotifyOfPropertyChange(() => OptionKeyId); }
		}


	}
}

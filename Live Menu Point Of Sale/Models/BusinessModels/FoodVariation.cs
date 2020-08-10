using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Live_Menu_Point_Of_Sale.Models.BusinessModels
{
	[Serializable]
    public class FoodVariation : PropertyChangedBase
    {

		public FoodVariation()
		{
			Color = Brushes.Transparent;
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

		private int _cpn;

		public int Cpn
		{
			get { return _cpn; }
			set { _cpn = value; NotifyOfPropertyChange(() => Cpn); }
		}

		private Guid _productId;

		public Guid ProductId
		{
			get { return _productId; }
			set { _productId = value; NotifyOfPropertyChange(() => ProductId); }
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

		private Brush _color;

		public Brush Color
		{
			get { return _color; }
			set { _color = value; NotifyOfPropertyChange(() => Color); }
		}


	}
}

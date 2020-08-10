using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Live_Menu_Point_Of_Sale.Models
{
	// currently UI object table is saving to database
    public class Table : PropertyChangedBase
    {

		public Table()
		{
			Color = Brushes.LawnGreen;
		}

		private Guid _id;

		public Guid Id
		{
			get { return _id; }
			set { _id = value; NotifyOfPropertyChange(() => Id); }
		}

		private int _seats;

		public int Seats
		{
			get { return _seats; }
			set { _seats = value; NotifyOfPropertyChange(() => Seats); }
		}

		private int _serving;

		public int Serving
		{
			get { return _serving; }
			set { _serving = value; NotifyOfPropertyChange(() => Serving);
				NotifyOfPropertyChange(() => Status);
			}
		}

		private Brush _color;

		public Brush Color
		{
			get 
			{
				_color = GetColor();
				return _color; 
			}
			set 
			{ 
				_color = GetColor();
				NotifyOfPropertyChange(() => Color);
			}
		}

		private bool _selected;

		public bool Selected
		{
			get { return _selected; }
			set 
			{
				_selected = value;
				NotifyOfPropertyChange(() => Selected);
				NotifyOfPropertyChange(() => Color);
			}
		}

		private Guid _cartId;

		public Guid CartId
		{
			get { return _cartId; }
			set { _cartId = value; NotifyOfPropertyChange(() => CartId); }
		}



		public Brush GetColor()
		{
			if (Selected)
			{
				return Brushes.Aquamarine;
			}

			if(Serving == 1)
			{
				return Brushes.PaleVioletRed;
			}

			return Brushes.LawnGreen;
		}

		private string _status;

		public string Status
		{
			get 
			{
				_status = GetStatus();
				return _status;
			}
			set { _status = GetStatus(); NotifyOfPropertyChange(() => Status); }
		}

		public string GetStatus()
		{
			if(Serving == 1)
			{
				return "Serving";
			}

			return "Free";
		}


	}
}

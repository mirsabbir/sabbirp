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
    public class FoodCategory : PropertyChangedBase
    {
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

		private Brush _color;

		public Brush Color
		{
			get { return _color; }
			set { _color = value; NotifyOfPropertyChange(() => Color); }
		}

		public FoodCategory()
		{
			Color = Brushes.LightGreen;
		}
	}
}

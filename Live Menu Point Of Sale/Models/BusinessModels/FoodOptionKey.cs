using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.Models.BusinessModels
{
	[Serializable]
    public class FoodOptionKey : PropertyChangedBase
    {
		public FoodOptionKey()
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


	}
}

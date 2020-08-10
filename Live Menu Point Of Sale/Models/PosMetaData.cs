using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.Models
{
    public class PosMetaData : PropertyChangedBase
    {
		private string _name;

		public string Name
		{
			get { return _name; }
			set { _name = value; NotifyOfPropertyChange(() => Name); }
		}

		private string _address;

		public string Address
		{
			get { return _address; }
			set { _address = value; NotifyOfPropertyChange(() => Address); }
		}

		private string _orderType;

		public string OrderType
		{
			get { return _orderType; }
			set { _orderType = value; NotifyOfPropertyChange(() => OrderType); }
		}

		private Table table;

		public Table Table
		{
			get { return table; }
			set { table = value; NotifyOfPropertyChange(() => Table); }
		}


	}
}

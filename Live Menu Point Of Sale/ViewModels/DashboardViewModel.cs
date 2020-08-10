using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class DashboardViewModel : Screen
    {
		private BindableCollection<int> myVar;

		public BindableCollection<int> MyProperty
		{
			get { return myVar; }
			set { myVar = value; NotifyOfPropertyChange(() => MyProperty); }
		}


		public DashboardViewModel()
		{
			MyProperty = new BindableCollection<int>();
			MyProperty.Add(1);
			MyProperty.Add(1);
			MyProperty.Add(1);
		}

	}
}

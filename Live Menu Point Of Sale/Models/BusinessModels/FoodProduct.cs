using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.Models.BusinessModels
{
	[Serializable]
    public class FoodProduct : PropertyChangedBase
    {
		public FoodProduct()
		{

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

		private int _order;

		public int Order
		{
			get { return _order; }
			set { _order = value; NotifyOfPropertyChange(() => Order); }
		}

		private string _description;

		public string Description
		{
			get { return _description; }
			set { _description = value; NotifyOfPropertyChange(() => Description); }
		}

		private BindableCollection<FoodVariation> _variations;

		public BindableCollection<FoodVariation> Variations
		{
			get { return _variations; }
			set { _variations = value; NotifyOfPropertyChange(() => Variations); }
		}

		private FoodVariation _selectedVariation;

		public FoodVariation SelectedVariation
		{
			get { return _selectedVariation; }
			set { _selectedVariation = value; NotifyOfPropertyChange(() => SelectedVariation); }
		}

		private ObservableDictionary<FoodOptionKey, Pair<FoodOptionValue, BindableCollection<FoodOptionValue>>> _options;

		public ObservableDictionary<FoodOptionKey, Pair<FoodOptionValue, BindableCollection<FoodOptionValue>>> Options
		{
			get { return _options; }
			set { _options = value; NotifyOfPropertyChange(() => Options); }
		}

		private Guid categoryId;

		public Guid CategoryId
		{
			get { return categoryId; }
			set { categoryId = value; NotifyOfPropertyChange(() => CategoryId); }
		}

	}
}

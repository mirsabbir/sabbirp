using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class CreateCategoryDialogViewModel : Screen
    {
        private bool _confirmed;

        public bool Confirmed
        {
            get { return _confirmed; }
            set { _confirmed = value; NotifyOfPropertyChange(() => Confirmed); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyOfPropertyChange(() => Name); }
        }


        public CreateCategoryDialogViewModel(string name = "")
        {
            Confirmed = false;
            Name = name;
        }

        public void AddCategory()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return;
            }

            Confirmed = true;
            TryClose();
        }

        public void Cancel()
        {
            Confirmed = false;
            TryClose();
        }
    }
}

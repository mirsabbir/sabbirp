using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class FreeTableConfirmDialogViewModel : Screen
    {
        private bool _confirmed;

        public bool Confirmed
        {
            get { return _confirmed; }
            set { _confirmed = value; NotifyOfPropertyChange(() => Confirmed); }
        }


        public FreeTableConfirmDialogViewModel()
        {
            Confirmed = false;
        }

        public void Ok()
        {
            Confirmed = true;
            this.TryClose();
        }

        public void Cancel()
        {
            this.TryClose();
        }

    }
}

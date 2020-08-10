using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class SeatInputDialogViewModel : Screen
    {
        private int _number;

        public int Number
        {
            get { return _number; }
            set { _number = value; NotifyOfPropertyChange(() => Number); }
        }

        private bool _confirmed;

        public bool Confirmed
        {
            get { return _confirmed; }
            set { _confirmed = value; NotifyOfPropertyChange(() => Confirmed); }
        }



        public SeatInputDialogViewModel(int initValue = 0)
        {
            Confirmed = false;
            Number = initValue;
        }

        public void PressBackSpace()
        {
            Number = Number / 10;
        }

        public void Press1()
        {
            Number = Number * 10 + 1;
        }

        public void Press2()
        {
            Number = Number * 10 + 2;
        }

        public void Press3()
        {
            Number = Number * 10 + 3;
        }

        public void Press4()
        {
            Number = Number * 10 + 4;
        }

        public void Press5()
        {
            Number = Number * 10 + 5;
        }

        public void Press6()
        {
            Number = Number * 10 + 6;
        }

        public void Press7()
        {
            Number = Number * 10 + 7;
        }

        public void Press8()
        {
            Number = Number * 10 + 8;
        }

        public void Press9()
        {
            Number = Number * 10 + 9;
        }

        public void Press0()
        {
            Number = Number * 10 ;
        }

        public void PressOk()
        {
            Confirmed = true;
            TryClose();
        }

    }
}

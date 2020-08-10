using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class DeliveryViewModel : Screen
    {
        private string _text;

        public string text
        {
            get { return _text; }
            set 
            { 
                _text = value; 
                NotifyOfPropertyChange(() => text);
                Name = text;
            }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyOfPropertyChange(() => Name); }
        }


        public DeliveryViewModel()
        {
            
        }

        public delegate void GoToOrderPageChosenEventHandler();
        public event GoToOrderPageChosenEventHandler GoToOrderPageChosenEvent;

        public void OnGoToOrderPageChosenEvent()
        {
            GoToOrderPageChosenEvent?.Invoke();
        }

        public void Delivery()
        {
            OnGoToOrderPageChosenEvent();
        }

        public void Collect()
        {
            OnGoToOrderPageChosenEvent();
        }

        public void PA() { text += 'A'; }
        public void PB() { text += 'B'; }
        public void PC() { text += 'C'; }
        public void PD() { text += 'D'; }
        public void PE() { text += 'E'; }
        public void PF() { text += 'F'; }
        public void PG() { text += 'G'; }
        public void PH() { text += 'H'; }
        public void PI() { text += 'I'; }
        public void PJ() { text += 'J'; }
        public void PK() { text += 'K'; }
        public void PL() { text += 'L'; }
        public void PM() { text += 'M'; }
        public void PN() { text += 'N'; }
        public void PO() { text += 'O'; }
        public void PP() { text += 'P'; }
        public void PQ() { text += 'Q'; }
        public void PR() { text += 'R'; }
        public void PS() { text += 'S'; }
        public void PT() { text += 'T'; }
        public void PU() { text += 'U'; }
        public void PV() { text += 'V'; }
        public void PW() { text += 'W'; }
        public void PX() { text += 'X'; }
        public void PY() { text += 'Y'; }
        public void PZ() { text += 'Z'; }
        public void P0() { text += '0'; }
        public void P1() { text += '1'; }
        public void P2() { text += '2'; }
        public void P3() { text += '3'; }
        public void P4() { text += '4'; }
        public void P5() { text += '5'; }
        public void P6() { text += '6'; }
        public void P7() { text += '7'; }
        public void P8() { text += '8'; }
        public void P9() { text += '9'; }
        public void PAT() { text += '@'; }
        public void PDOT() { text += '.'; }
        public void PSPACE() { text += ' '; }
        public void PDEL() 
        {
            if (text.Length == 0) return;
            
            text = new string(text.Reverse().ToArray());
            text = text.Substring(1);
            text = new string(text.Reverse().ToArray());
        }
        public void PCROSS()
        {
            text = string.Empty;
        }

        public void PENTER()
        {
            
        }


    }
}

using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.DataStructure
{
    public class Pair<T1, T2> : PropertyChangedBase
    {
        private T1 _first;

        public T1 First
        {
            get { return _first; }
            set { _first = value; NotifyOfPropertyChange(() => First); }
        }

        private T2 _second;

        public T2 Second
        {
            get { return _second; }
            set { _second = value; NotifyOfPropertyChange(() => Second); }
        }
    }
}

using Caliburn.Micro;
using Live_Menu_Point_Of_Sale.Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.Models
{
    /// <summary>
    /// This the cart item object.
    /// </summary>
    /// <seealso cref="Caliburn.Micro.PropertyChangedBase" />
    public class CartObject : PropertyChangedBase
    {
        #region constructor

        public CartObject()
        {

        }

        #endregion

        #region properties

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

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; NotifyOfPropertyChange(() => Description); }
        }


        private string _note;

        public string Note
        {
            get { return _note; }
            set { _note = value; NotifyOfPropertyChange(() => Note); }
        }


        private int _count;

        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                NotifyOfPropertyChange(() => Count);
                NotifyOfPropertyChange(() => TotalPrice);
            }
        }

        private double _totalPrice;

        public double TotalPrice
        {
            get { return GetPrice(); }
            set { _totalPrice = GetPrice(); NotifyOfPropertyChange(() => TotalPrice); }
        }

        private Variation _variation;

        public Variation Variation
        {
            get { return _variation; }
            set { _variation = value; NotifyOfPropertyChange(() => Variation); }
        }


        #endregion

        #region methods

        public double GetPrice()
        {
            return 0;
        }

        #endregion

    }
}

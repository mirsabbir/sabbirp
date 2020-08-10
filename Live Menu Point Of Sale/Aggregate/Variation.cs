using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.Aggregate
{
    public class Variation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Cpn { get; set; }

        public int Order { get; set; }

        public double DeliveryPrice { get; set; }

        public double CollectionPrice { get; set; }

        public double DineInPrice { get; set; }

        public Guid ProductId { get; set; }
    }
}

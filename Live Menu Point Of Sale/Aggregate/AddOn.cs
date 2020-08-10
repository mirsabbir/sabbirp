using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.Aggregate
{
    public class AddOn
    {
        public Guid Id { get; set; }

        public Guid VariationId { get; set; }

        public string Name { get; set; }

        public double DeliveryPrice { get; set; }

        public double CollectionPrice { get; set; }

        public double DineInPrice { get; set; }
    }
}

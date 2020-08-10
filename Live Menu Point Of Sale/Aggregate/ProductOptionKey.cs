using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.Aggregate
{
    public class ProductOptionKey
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid OptionKeyId { get; set; }
    }
}

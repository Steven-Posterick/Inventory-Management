using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory_Management.Model
{
    public partial class Product
    {
        public Product()
        {
            Records = new HashSet<Record>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? Cost { get; set; }

        public virtual ReceivedRecord ReceivedRecord { get; set; }
        public virtual ICollection<Record> Records { get; set; }
    }
}

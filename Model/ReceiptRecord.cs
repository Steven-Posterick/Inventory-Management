using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory_Management.Model
{
    public partial class ReceiptRecord
    {
        public ReceiptRecord()
        {
            Allocations = new HashSet<Allocation>();
        }

        public int Id { get; set; }
        public decimal? Price { get; set; }

        public virtual Record IdNavigation { get; set; }
        public virtual ICollection<Allocation> Allocations { get; set; }
    }
}

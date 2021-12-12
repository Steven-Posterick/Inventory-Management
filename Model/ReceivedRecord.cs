using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory_Management.Model
{
    public partial class ReceivedRecord
    {
        public ReceivedRecord()
        {
            Allocations = new HashSet<Allocation>();
        }

        public int Id { get; set; }
        public decimal? Cost { get; set; }

        public virtual Product IdNavigation { get; set; }
        public virtual ICollection<Allocation> Allocations { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory_Management.Model
{
    public partial class Allocation
    {
        public int ReceiptId { get; set; }
        public int ReceivedId { get; set; }
        public int? AllocatedQuantity { get; set; }

        public virtual ReceiptRecord Receipt { get; set; }
        public virtual ReceivedRecord Received { get; set; }
    }
}

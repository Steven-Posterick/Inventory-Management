using System;
using System.Collections.Generic;

#nullable disable

namespace Inventory_Management.Model
{
    public partial class Record
    {
        public int RecordId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? Date { get; set; }

        public virtual Product Product { get; set; }
        public virtual ReceivedRecord Record1 { get; set; }
        public virtual ReceiptRecord RecordNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal ProductPrice { get; set; }
        public string? ProductDescription { get; set; }
        public short Inventory { get; set; }
        public short UnitsOnOrder { get; set; }
        public bool IsDiscontinued { get; set; }
        public int CategoryId { get; set; }
        public int? GameId { get; set; }
        public int? SupplierId { get; set; }
        public string? ProductImg { get; set; }

        public virtual ProductCategory? Category { get; set; }
        public virtual Game? Game { get; set; }
        public virtual Supplier? Supplier { get; set; }
    }
}

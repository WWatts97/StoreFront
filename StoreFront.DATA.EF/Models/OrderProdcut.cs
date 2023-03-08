﻿using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class OrderProdcut
    {
        public int OrderProductId { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public short? Quantity { get; set; }
        public decimal? ProductPrice { get; set; }
    }
}

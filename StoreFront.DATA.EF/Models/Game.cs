using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Game
    {
        public Game()
        {
            Products = new HashSet<Product>();
        }

        public int GameId { get; set; }
        public string GameName { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}

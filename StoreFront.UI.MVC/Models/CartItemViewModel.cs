using StoreFront.DATA.EF.Models;

namespace StoreFront.UI.MVC.Models
{
    public class CartItemViewModel
    {
        public int Qty { get; set; }

        public Product Product { get; set; }

        //Fully-Qualified Ctor
        public CartItemViewModel(int qty, Product product)
        {
            //Assignment
            //prop = param
            Qty = qty;
            Product = product;
        }
    }
}

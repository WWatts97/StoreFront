using Microsoft.AspNetCore.Mvc;
using StoreFront.DATA.EF.Models;//Access to Entity Models
using Microsoft.AspNetCore.Identity; //Access to Identity lasses
using StoreFront.UI.MVC.Models; //Access to CartItemViewModel
using Newtonsoft.Json;

namespace StoreFront.UI.MVC.Controllers
{
    
    public class ShoppingCartController : Controller
    {
        //Fields
        private readonly HobbyshopContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        //Ctor
        public ShoppingCartController(HobbyshopContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var sessionCart = HttpContext.Session.GetString("cart");

            Dictionary<int, CartItemViewModel> shoppingCart = null;

            if (sessionCart == null || sessionCart.Count() == 0)
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();

                ViewBag.Message = "There are no items in your cart.";
            }
            else
            {
                ViewBag.Message = null;
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }
            return View(shoppingCart);
        }

        public IActionResult AddToCart(int id)
        {
            //Create an empty shell for our LOCAL shopping cart variable
            //int (key) > ProductID
            //CartItemViewModel (value) > Product & Qty

            Dictionary<int, CartItemViewModel> shoppingCart = null!;

            #region Session Notes
            /*
             * Session is a tool available on the server-side that can store information for a user while they are actively using your site.
             * Typically the session lasts for 20 minutes (this can be adjusted in Program.cs).
             * Once the 20 minutes is up, the session variable is disposed.
             * 
             * Values that we can store in Session are limited to: string, int
             * - Because of this we have to get creative when trying to store complex objects (like CartItemViewModel).
             * To keep the info separated into properties we will convert the C# object to a JSON string.
             * */
            #endregion

            var sessionCart = HttpContext.Session.GetString("cart");

            //check to see if the Session cart exists
            //If not, instantiate a new local collection
            if (String.IsNullOrEmpty(sessionCart))
            {
                //If the session cart didn't exist yet, instantiate a collection as a new object
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }
            else
            {
                //Cart already exists. So, transfer the existing cart itrems from the sessioncart into our local shoppingCart

                //DeserializeObject() is a method that converts JSON to C#. We MUST tell this method
                //which C# class to convert the JSON into (here, Dictionary<int>, CartItemViewModel>)
                shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);
            }

            //Add the newly selected Products to the cart
            //Retriee the Product from the DB

            Product product = _context.Products.Find(id);

            //instantiate the CartItemViewModel object so we can add it to the cart
            CartItemViewModel civm = new CartItemViewModel(1, product);//Adds 1 of the selected Product to the cart

            //if the product was already in the cart, increase the qty by 1.
            //otherwise, add the new item to the cart
            if (shoppingCart.ContainsKey(product.ProductId))
            {
                //item is already in the cart -- increase the qty
                shoppingCart[product.ProductId].Qty++;
            }
            else
            {
                //Item wasn't in the cart -- add it
                shoppingCart.Add(product.ProductId, civm);
            }

            //Update the session version of the cart
            //take the local copy and serialize it as JSON
            //finna;ly, assign that value to our session
            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            //Retrieve the cart from the session
            var sessionCart = HttpContext.Session.GetString("cart");

            //Convert the JSON to C#
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            //Remove item from the cart
            shoppingCart.Remove(id);

            //If there are no remaining items, remove it form the Session
            if (shoppingCart.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }
            else
            {
                //Otherwise, update the session variable with our updated cart contents
                string jsonCart = JsonConvert.SerializeObject(shoppingCart);
                HttpContext.Session.SetString("cart", jsonCart);
            }

            return RedirectToAction("Index");
        }
        public IActionResult UpdateCart(int productId, int qty)
        {
            //Retieve the cart
            var sessionCart = HttpContext.Session.GetString("cart");

            //Deserialize the cart from JSON to C#
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            //Update the qty for the specific dictionary key
            shoppingCart[productId].Qty = qty;

            //Update the session
            string jsonCart = JsonConvert.SerializeObject(shoppingCart);
            HttpContext.Session.SetString("cart", jsonCart);

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> SubmitOrder()
        {
            #region Planning Out Order Submission

            //BIG PICTURE PLAN
            //Create the Order object and save it to the database
            // -Order date
            // - UserID
            // - Shipping info props... can pull this info from the UserDetails
            //Add the record to the database _context
            //Save the changes

            //Create the OrderProducts object for each item in the cart
            // - ProductID > from the cart
            // - OrderID > from the order object
            // - Qty > from the cart
            // - ProductPrice > from the cart

            #endregion

            //Retrieve the current user's ID
            //This is a mechanism provided by Identity to retriece the USerID in the current HttpContext.
            //If you need to retrieve the UserID in ANY Controller, you can use this:
            string? userId = (await _userManager.GetUserAsync(HttpContext.User))?.Id;

            //Retrieve the UserDetails record
            UserDetail ud = _context.UserDetails.Find(userId);

            //Create the Order object and assign values accordingly
            Order o = new Order()
            {
                OrderDate = DateTime.Now,
                UserId = userId,
                ShipCity = ud.City,
                ShipToName = ud.FirstName + " " + ud.LastName,
                ShipState = ud.State,
                ShipZip = ud.Zip
            };

            //Add the Order to the _context
            _context.Orders.Add(o);

            //Retrieve the session cart
            var sessionCart = HttpContext.Session.GetString("cart");

            //Convert
            Dictionary<int, CartItemViewModel> shoppingCart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(sessionCart);

            //Create an OrderProduct object for each item in the cart
            foreach (var item in shoppingCart)
            {
                OrderProduct op = new OrderProduct()
                {
                    OrderId = o.OrderId,
                    ProductId = item.Key,
                    ProductPrice = item.Value.Product.ProductPrice,
                    Quantity = (short)item.Value.Qty
                };

                //We only need to add items to an existing entity if the items are a related record
                //(like frim a linking table)
                o.OrderProducts.Add(op);


            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Orders");
        }
    }
}

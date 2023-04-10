using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using GadgetStore.UI.MVC.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.DATA.EF.Models;
using System.Drawing;//added for image-related classes
using GadgetStore.UI.MVC.Utilities;
using X.PagedList; //access to PagedList classes.

namespace StoreFront.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly HobbyshopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(HobbyshopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var hobbyshopContext = _context.Products.Include(p => p.Category).Include(p => p.Game).Include(p => p.Supplier);
            return View(await hobbyshopContext.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> TiledProducts(string searchTerm, int categoryId = 0, int page = 1, int gameId = 0)
        {
            int pageSize = 8;

            var products = _context.Products
                .Include(p => p.Category).Include(p => p.Game).Include(p => p.Supplier).ToList();

            ViewData["CategoryId"] = new SelectList(_context.ProductCategories, "CategoryId", "CategoryName");
            ViewBag.Category = 0;

            if (categoryId != 0)
            {
                products = products.Where(p => p.CategoryId == categoryId).ToList();

                ViewData["CategoryId"] = new SelectList(_context.ProductCategories, "CategoryId", "CategoryName", categoryId);

                ViewBag.Category = categoryId;
            }

            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameName");
            ViewBag.Game = 0;

            if (gameId != 0)
            {
                products = products.Where(p => p.GameId == gameId).ToList();

                ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameName", gameId);

                ViewBag.Game = gameId;
            }

            if (!String.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p =>
                 p.ProductName.ToLower().Contains(searchTerm.ToLower()) ||
                 p.Game.GameName.ToLower().Contains(searchTerm.ToLower()) ||
                 p.Category.CategoryName.ToLower().Contains(searchTerm.ToLower()) ||
                 p.ProductDescription.ToLower().Contains(searchTerm.ToLower())).ToList();

                ViewBag.NbrResults = products.Count;

                ViewBag.SearchTerm = searchTerm;
            }

            return View(products.ToPagedList(page, pageSize));
        }

        public async Task<IActionResult> PopularProducts()
        {
            var products = _context.Products
                .Include(p => p.Category).Include(p => p.Game).Include(p => p.Supplier)
                .OrderBy(p => p.UnitsOnOrder).ToList();
            return View(await products.Take(8).ToListAsync());
        }

        [AllowAnonymous]
        public PartialViewResult ProductDetails(int id)
        {
            var product = _context.Products.Find(id);
            return PartialView(product);
        }

        // GET: Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Game)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {

            ViewData["CategoryId"] = new SelectList(_context.ProductCategories, "CategoryId", "CategoryName");
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameName");
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductPrice,ProductDescription,Inventory,UnitsOnOrder,IsDiscontinued,CategoryId,GameId,SupplierId,ProductImg,Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                #region File Upload-Create
                //ONLY BOTHER IF MODEL IS VALID check to see if file was upoloaded
                if (product.Image != null)
                {
                    //there is a file uploaded
                    //check the file type
                    string ext = Path.GetExtension(product.Image.FileName);

                    //create a list of acceptable to check against
                    string[] validExts = { ".jpeg", ".jpg", ".gif", ".png", ".webp" };

                    //verify the uploaded file has a matching extension
                    //AND verify the file size will work with our .NET app
                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        //generate unique filename GUID
                        product.ProductImg = Guid.NewGuid() + ext;
                        //to save this file to the right folder on the webserver
                        //to access the web root use the _webHostEnviroment
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string fullImagePath = webRootPath + "/images/";//gets us to wwwroot/images

                        //create a MemoryStream to read the image into server memory
                        using (var memoryStream = new MemoryStream())
                        {
                            await product.Image.CopyToAsync(memoryStream);//transfer file from request to server memorty

                            using (var img = Image.FromStream(memoryStream))//setting up an image not a file
                            {
                                //now send the image to ImageUtility to resize for base file and also thumbnail
                                int maxImageSize = 500;//in pixles
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullImagePath, product.ProductImg, img, maxImageSize, maxThumbSize);//resixed and saved
                            }
                        }
                    }
                }
                else
                {
                    //in this case no file was uploaded, so assign a default file name
                    //IF YOU HAVEN'T ALREADY, GET THE DEFAULT FILE
                    product.ProductImg = "NoImage.png";
                }
                #endregion

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.ProductCategories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameName", product.GameId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.ProductCategories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameName", product.GameId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }


        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductPrice,ProductDescription,Inventory,UnitsOnOrder,IsDiscontinued,CategoryId,GameId,SupplierId,ProductImg,Image")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                #region EDIT File Upload
                //retain old image file name so we can delete if a new file was uploaded
                string oldImageName = product.ProductImg;

                //Check if the user uploaded a file
                if (product.Image != null)
                {
                    //get the file's extension
                    string ext = Path.GetExtension(product.Image.FileName);

                    //list valid extensions
                    string[] validExts = { ".jpeg", ".jpg", ".png", ".gif", ".webp" };

                    //check the file's extension against the list of valid extensions
                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        //generate a unique file name
                        product.ProductImg = Guid.NewGuid() + ext;
                        //build our file path to save the image
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string fullPath = webRootPath + "/images/";

                        //Delete the old image
                        if (oldImageName != "NoImage.png")
                        {
                            ImageUtility.Delete(fullPath, oldImageName);
                        }

                        //Save the new image to webroot
                        using (var memoryStream = new MemoryStream())
                        {
                            await product.Image.CopyToAsync(memoryStream);
                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 500;
                                int maxThumbSize = 100;
                                ImageUtility.ResizeImage(fullPath, product.ProductImg, img, maxImageSize, maxThumbSize);
                            }
                        }

                    }
                }
                #endregion

                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.ProductCategories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "GameName", product.GameId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }



        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Game)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'GadgetStoreContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null && product.ProductImg != "NoImage.png")
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string fullImagePath = webRootPath + "/images/";
                ImageUtility.Delete(fullImagePath, product.ProductImg);
                _context.Products.Remove(product);
            }
            else
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //NOT PART OF THE DELETE
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}

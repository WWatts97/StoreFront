using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreFront.DATA.EF.Models;

namespace StoreFront.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly HobbyshopContext _context;

        public OrdersController(HobbyshopContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var hobbyshopContext = _context.Orders.Include(s => s.User);
            return View(await hobbyshopContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var Orders = await _context.Orders
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (Orders == null)
            {
                return NotFound();
            }

            return View(Orders);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "UserId");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,UserId,OrderDate,ShipToName,ShipCity,ShipState,ShipZip")] Order Orders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Orders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "UserId", Orders.UserId);
            return View(Orders);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var Orders = await _context.Orders.FindAsync(id);
            if (Orders == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "UserId", Orders.UserId);
            return View(Orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,UserId,OrderDate,ShipToName,ShipCity,ShipState,ShipZip")] Order Orders)
        {
            if (id != Orders.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(Orders.OrderId))
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
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "UserId", Orders.UserId);
            return View(Orders);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var Orders = await _context.Orders
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (Orders == null)
            {
                return NotFound();
            }

            return View(Orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'HobbyshopContext.Orders'  is null.");
            }
            var Orders = await _context.Orders.FindAsync(id);
            if (Orders != null)
            {
                _context.Orders.Remove(Orders);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
          return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}

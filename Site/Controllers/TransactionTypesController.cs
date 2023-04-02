using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Models;

namespace Site.Controllers
{
    public class TransactionTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TransactionTypes
        public async Task<IActionResult> Index()
        {
              return _context.TransactionTypes != null ? 
                          View(await _context.TransactionTypes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.TransactionTypes'  is null.");
        }

        // GET: TransactionTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TransactionTypes == null)
            {
                return NotFound();
            }

            var transactionTypes = await _context.TransactionTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transactionTypes == null)
            {
                return NotFound();
            }

            return View(transactionTypes);
        }

        // GET: TransactionTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TransactionTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] TransactionTypes transactionTypes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transactionTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transactionTypes);
        }

        // GET: TransactionTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TransactionTypes == null)
            {
                return NotFound();
            }

            var transactionTypes = await _context.TransactionTypes.FindAsync(id);
            if (transactionTypes == null)
            {
                return NotFound();
            }
            return View(transactionTypes);
        }

        // POST: TransactionTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] TransactionTypes transactionTypes)
        {
            if (id != transactionTypes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transactionTypes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionTypesExists(transactionTypes.Id))
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
            return View(transactionTypes);
        }

        // GET: TransactionTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TransactionTypes == null)
            {
                return NotFound();
            }

            var transactionTypes = await _context.TransactionTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transactionTypes == null)
            {
                return NotFound();
            }

            return View(transactionTypes);
        }

        // POST: TransactionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TransactionTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TransactionTypes'  is null.");
            }
            var transactionTypes = await _context.TransactionTypes.FindAsync(id);
            if (transactionTypes != null)
            {
                _context.TransactionTypes.Remove(transactionTypes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionTypesExists(int id)
        {
          return (_context.TransactionTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

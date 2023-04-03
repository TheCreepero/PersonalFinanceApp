using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Models;
using Site.Models.ViewModels;

namespace Site.Controllers
{
    public class BudgetsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private NumberFormatInfo customFormat = new NumberFormatInfo
        {
            NumberDecimalSeparator = ","
        };

        public BudgetsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Budgets
        public async Task<IActionResult> Index()
        {
            // Fetch all budgets, transaction types, and months asynchronously
            var budgets = await _context.Budget.ToListAsync();
            var transactionTypes = await _context.TransactionTypes.ToDictionaryAsync(x => x.Id, x => x.Name);
            var months = await _context.Month.ToDictionaryAsync(x => x.MonthId, x => x.MonthName);

            // Initialize the view model
            var viewModel = new List<IndexBudgetViewModel>();

            // Populate the view model using fetched data
            foreach (var budget in budgets)
            {
                var budgetModel = new IndexBudgetViewModel
                {
                    TypeName = transactionTypes[budget.TypeId],
                    MonthName = months[budget.MonthId],
                    Amount = budget.Amount.ToString(),
                    Id = budget.Id
                };
                viewModel.Add(budgetModel);
            }

            return View(viewModel);
        }

        // GET: Budgets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Budget == null)
            {
                return NotFound();
            }

            var budget = await _context.Budget
                .FirstOrDefaultAsync(m => m.Id == id);
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
        }

        // GET: Budgets/Create
        public IActionResult Create()
        {
            ViewBag.Types = _context.TransactionTypes.ToList();
            ViewBag.Months = _context.Month.ToList();
            var viewModel = new BudgetViewModel();
            return View(viewModel);
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeId,MonthId,Amount")] BudgetViewModel budget)
        {
            decimal budgetAmount;
            if (decimal.TryParse(budget.Amount.Replace('.', ','), NumberStyles.Any, customFormat, out budgetAmount))
            {
                var model = new Budget
                {
                    TypeId = budget.TypeId,
                    MonthId = budget.MonthId,
                    Amount = budgetAmount,
                };

                if (ModelState.IsValid)
                {
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(budget);
        }

        // GET: Budgets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Budget == null)
            {
                return NotFound();
            }

            var budget = await _context.Budget.FindAsync(id);
            if (budget == null)
            {
                return NotFound();
            }

            var viewModel = new BudgetViewModel
            {
                Id = budget.Id,
                MonthId = budget.MonthId,
                TypeId = budget.TypeId,
                Amount = budget.Amount.ToString()
            };

            ViewBag.Types = _context.TransactionTypes.ToList();
            ViewBag.Months = _context.Month.ToList();
            return View(viewModel);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeId,MonthId,Amount")] BudgetViewModel budget)
        {
            if (id != budget.Id)
            {
                return NotFound();
            }

            decimal budgetAmount;
            if (decimal.TryParse(budget.Amount.Replace('.', ','), NumberStyles.Any, customFormat, out budgetAmount))
            {
                var model = new Budget
                {
                    Id = id,
                    TypeId = budget.TypeId,
                    MonthId = budget.MonthId,
                    Amount = budgetAmount
                };

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(model);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BudgetExists(budget.Id))
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
            }
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Budget == null)
            {
                return NotFound();
            }

            var budget = await _context.Budget
                .FirstOrDefaultAsync(m => m.Id == id);
            if (budget == null)
            {
                return NotFound();
            }

            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Budget == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Budget'  is null.");
            }
            var budget = await _context.Budget.FindAsync(id);
            if (budget != null)
            {
                _context.Budget.Remove(budget);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BudgetExists(int id)
        {
            return (_context.Budget?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
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
    public class MonthsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonthsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Months
        public async Task<IActionResult> Index()
        {
            return _context.Month != null ?
                        View(await _context.Month.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Month'  is null.");
        }

        // GET: Months/Details/5
        public async Task<IActionResult> Details(int year, int month)
        {
            var monthDate = new DateTime(year, month, 1);

            var transactions = _context.Transaction
                .Where(t => t.Date.Year == year && t.Date.Month == month)
                .OrderBy(t => t.Date)
                .ToList();

            // Load the related Account entities explicitly
            var accountIds = transactions.Select(t => t.AccountId).Distinct();
            var accounts = _context.Account
                .Where(a => accountIds.Contains(a.AccountId))
                .ToList();

            var transactionDetails = transactions
                .Join(accounts, t => t.AccountId, a => a.AccountId, (t, a) => new TransactionDetail
                {
                    Transaction = t,
                    Account = a
                })
                .ToList();

            // Calculate the account balances for the specified month
            var accountBalances = accounts.ToDictionary(a => a.AccountId, a => a.AccountBalance);

            var summaryTable = transactionDetails
                .GroupBy(td => td.Transaction.AccountId)
                .Select(g => new AccountSummary
                {
                    AccountName = g.First().Account.AccountName,
                    BeginningBalance = accountBalances[g.Key] + g.Where(td => td.Transaction.Date < monthDate).Sum(td => td.Transaction.TransactionAmount),
                    EndingBalance = accountBalances[g.Key] + g.Where(td => td.Transaction.Date <= monthDate.AddMonths(1).AddDays(-1)).Sum(td => td.Transaction.TransactionAmount),
                    TotalSpent = g.Where(td => td.Transaction.TransactionAmount < 0).Sum(td => td.Transaction.TransactionAmount) * -1
                })
                .ToList();

            //var accountBalances = _context.Account.Where(a => accountIds.Contains(a.AccountId)).ToDictionary(a => a.AccountId, a => a.AccountBalance);

            var accountNames = _context.Account.Where(a => accountIds.Contains(a.AccountId)).ToDictionary(a => a.AccountId, a => a.AccountName);

            foreach (var transaction in transactions)
            {
                accountBalances[transaction.AccountId] += transaction.TransactionAmount;
            }

            var viewModel = new MonthTransactionViewModel
            {
                Month = monthDate,
                Transactions = transactions,
                SummaryTable = summaryTable,
                AccountBalances = accountBalances,
                AccountNames = accountNames
            };

            return View(viewModel);
        }

        // GET: Months/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Months/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MonthId,MonthName,StartDate,EndDate,StartBalance,EndBalance")] Month month)
        {
            if (ModelState.IsValid)
            {
                _context.Add(month);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(month);
        }

        // GET: Months/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Month == null)
            {
                return NotFound();
            }

            var month = await _context.Month.FindAsync(id);
            if (month == null)
            {
                return NotFound();
            }
            return View(month);
        }

        // POST: Months/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MonthId,MonthName,StartDate,EndDate,StartBalance,EndBalance")] Month month)
        {
            if (id != month.MonthId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(month);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonthExists(month.MonthId))
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
            return View(month);
        }

        // GET: Months/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Month == null)
            {
                return NotFound();
            }

            var month = await _context.Month
                .FirstOrDefaultAsync(m => m.MonthId == id);
            if (month == null)
            {
                return NotFound();
            }

            return View(month);
        }

        // POST: Months/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Month == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Month'  is null.");
            }
            var month = await _context.Month.FindAsync(id);
            if (month != null)
            {
                _context.Month.Remove(month);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonthExists(int id)
        {
            return (_context.Month?.Any(e => e.MonthId == id)).GetValueOrDefault();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Site.Data;
using Site.Models;
using Site.Models.ViewModels;

namespace Site.Controllers
{
    public class MonthsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SiteSettings _siteSettings;

        public MonthsController(ApplicationDbContext context, IOptions<SiteSettings> siteSettings)
        {
            _context = context;
            _siteSettings = siteSettings.Value;
        }

        // GET: Months
        public async Task<IActionResult> Index()
        {
            return _context.Month != null ?
                        View(await _context.Month.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Month'  is null.");
        }

        // GET: Months/Details/5
        public async Task<IActionResult> Details(int year, int month, int day, int? endMonth, int? endDay, int? endYear)
        {
            // Create start and end date objects for the given period
            var monthDate = new DateTime(year, month, day);
            var endDate = endYear.HasValue && endMonth.HasValue && endDay.HasValue ? new DateTime(endYear.Value, endMonth.Value, endDay.Value) : monthDate.AddMonths(1).AddDays(-1);

            // Fetch transactions within the specified period
            var transactions = await _context.Transaction
                .Where(t => t.Date >= monthDate && t.Date <= endDate)
                .OrderBy(t => t.Date)
                .ToListAsync();

            //Fetch an alternative list of all transactions, to be used in summaryTable creation
            var allTransactions = await _context.Transaction.ToListAsync();

            // Get distinct account IDs from the transactions
            var accountIds = transactions.Select(t => t.AccountId).Distinct();

            // Fetch accounts associated with the transactions
            var accounts = await _context.Account
                .Where(a => accountIds.Contains(a.AccountId))
                .ToListAsync();

            // Join transactions and accounts into transaction details
            var transactionDetails = transactions
                .Join(accounts, t => t.AccountId, a => a.AccountId, (t, a) => new TransactionDetail
                {
                    Transaction = t,
                    Account = a
                })
                .ToList();

            //Alternative transactionDetails containing every transaction
            var allTransactionDetails = allTransactions
                .Join(accounts, t => t.AccountId, a => a.AccountId, (t, a) => new TransactionDetail
                {
                    Transaction = t,
                    Account = a
                })
                .ToList();

            // Create dictionaries for account balances and names
            var accountBalances = accounts.ToDictionary(a => a.AccountId, a => a.AccountBalance);
            var accountNames = accounts.ToDictionary(a => a.AccountId, a => a.AccountName);

            // Generate summary table data
            var summaryTable = allTransactionDetails
                .GroupBy(td => td.Transaction.AccountId)
                .Select(g => new AccountSummary
                {
                    AccountName = g.First().Account.AccountName,
                    BeginningBalance = g.Where(td => td.Transaction.Date < monthDate).Sum(td => td.Transaction.TransactionAmount),
                    EndingBalance = g.Where(td => td.Transaction.Date <= endDate).Sum(td => td.Transaction.TransactionAmount),
                    TotalSpent = g.Where(td => (td.Transaction.TransactionAmount < 0) && (td.Transaction.Date >= monthDate) && (td.Transaction.Date <= endDate)).Sum(td => td.Transaction.TransactionAmount) * -1
                })
                .ToList();

            // Populate the view model
            var viewModel = new MonthTransactionViewModel
            {
                Month = monthDate,
                Transactions = transactions,
                SummaryTable = summaryTable,
                AccountBalances = accountBalances,
                AccountNames = accountNames
            };

            // Set the currency symbol from site settings
            ViewData["CurrencySymbol"] = _siteSettings.CurrencySymbol;

            // Return the view with the view model
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Site.Data;
using Site.Models;
using Site.Models.ViewModels;
using Site.Utility;
using System.Globalization;

namespace Site.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AccountService _accountService;

        private CultureInfo culture = new CultureInfo("fi-FI");

        private NumberFormatInfo customFormat = new NumberFormatInfo
        {
            NumberDecimalSeparator = ","
        };

        public TransactionsController(ApplicationDbContext context, AccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var transactionsList = await _context.Transaction
                .Join(_context.Account,
                    transaction => transaction.AccountId,
                    account => account.AccountId,
                    (transaction, account) => new IndexTransactionViewModel
                    {
                        TransactionName = transaction.TransactionType,
                        TransactionAmount = transaction.TransactionAmount,
                        TransactionDate = transaction.Date,
                        TransactionId = transaction.TransactionId,
                        AccountName = account.AccountName,
                        Type = transaction.Type
                    })
                .ToListAsync();

            return View(transactionsList);
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewBag.Accounts = _context.Account.ToList();
            ViewBag.TransactionTypes = _context.TransactionTypes.ToList();

            var model = new CreateTransactionViewModel
            {
            };
            return View(model);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTransactionViewModel transaction)
        {
            // Fetch account and transaction type asynchronously
            var accountTask = _context.Account.FirstOrDefaultAsync(x => x.AccountId == transaction.SelectedAccount);
            var transactionTypeTask = _context.TransactionTypes.FirstOrDefaultAsync(x => x.Id == transaction.SelectedType);

            // Wait for tasks to complete
            var account = await accountTask;
            var transactionType = await transactionTypeTask;

            decimal transactionAmount;

            if (decimal.TryParse(transaction.TransactionAmount.Replace('.', ','), NumberStyles.Any, customFormat, out transactionAmount))
            {
                var model = new Transaction
                {
                    TransactionAmount = transactionAmount,
                    TransactionType = transaction.TransactionName,
                    AccountId = transaction.SelectedAccount,
                    Date = DateTime.Now,
                    Type = transactionType.Name
                };

                bool balanceUpdated = await _accountService.UpdateAccountBalance(account.AccountId, transactionAmount);

                if (ModelState.IsValid && balanceUpdated)
                {
                    _context.Add(model);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(t => t.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            var model = new TransactionViewModel
            {
                TransactionAmount = transaction.TransactionAmount.ToString(),
                TransactionName = transaction.TransactionType,
                TransactionDate = transaction.Date,
                TransactionId = transaction.TransactionId
            };

            ViewBag.Accounts = _context.Account.ToList();
            ViewBag.TransactionTypes = _context.TransactionTypes.ToList();
            return View(model);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, TransactionViewModel transaction)
        {
            if (id != transaction.TransactionId)
            {
                return NotFound();
            }
            var account = await _context.Account.FirstOrDefaultAsync(x => x.AccountId == transaction.SelectedAccount);

            decimal transactionAmount;
            if (decimal.TryParse(transaction.TransactionAmount, NumberStyles.Any, customFormat, out transactionAmount))
            {
                var model = new Transaction
                {
                    TransactionAmount = transactionAmount,
                    TransactionType = transaction.TransactionName,
                    Date = transaction.TransactionDate,
                    TransactionId = transaction.TransactionId,
                    AccountId = transaction.SelectedAccount,
                    Type = _context.TransactionTypes.FirstOrDefault(x => x.Id == transaction.SelectedType).Name
                };

                bool balanceUpdated = await _accountService.UpdateAccountBalance(account.AccountId, transactionAmount);

                if (ModelState.IsValid && balanceUpdated)
                {
                    try
                    {
                        _context.Update(model);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TransactionExists(model.TransactionId))
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
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction != null)
            {
                var account = _context.Account.FirstOrDefault(x => x.AccountId == transaction.AccountId);

                bool balanceUpdated = await _accountService.UpdateAccountBalance(account.AccountId, transaction.TransactionAmount);

                if (balanceUpdated)
                {
                    _context.Transaction.Remove(transaction);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(string id)
        {
            return (_context.Transaction?.Any(e => e.TransactionId == id)).GetValueOrDefault();
        }
    }
}
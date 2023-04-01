using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Site.Data;

namespace Site.Utility
{
    public class AccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateAccountBalance(int id, decimal balanceToAdd)
        {
            try
            {
                var account = await _context.Account.FirstOrDefaultAsync(x => x.AccountId == id);

                if (account == null)
                {
                    return false;
                }

                account.AccountBalance += balanceToAdd;

                _context.Update(account);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CalculateAccountBalance()
        {
            try
            {
                var accounts = await _context.Account.ToListAsync();
                var transactions = await _context.Transaction.ToListAsync();

                foreach (var account in accounts)
                {
                    var accountTransactions = transactions.Where(t => t.AccountId == account.AccountId);
                    var balance = accountTransactions.Sum(t => t.TransactionAmount);

                    account.AccountBalance = balance;
                    _context.Update(account);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Dictionary<int, decimal>> GetStartingBalances(DateTime startDate)
        {
            var accountBalances = await GetAccountBalances(startDate);
            var startingBalances = new Dictionary<int, decimal>();

            foreach (var accountBalance in accountBalances)
            {
                var account = await _context.Account.FindAsync(accountBalance.Key);
                var transactions = await _context.Transaction
                    .Where(t => t.AccountId == accountBalance.Key && t.Date < startDate)
                    .ToListAsync();

                decimal startingBalance = accountBalance.Value + account.AccountBalance;

                if (transactions.Count() > 0)
                {
                    startingBalance += transactions.Sum(t => t.TransactionAmount);
                }

                startingBalances[accountBalance.Key] = startingBalance;
            }

            return startingBalances;
        }

        public async Task<Dictionary<int, decimal>> GetAccountBalances(DateTime startDate)
        {
            var accountBalances = new Dictionary<int, decimal>();

            var accounts = await _context.Account.ToListAsync();
            foreach (var account in accounts)
            {
                decimal balance = account.AccountBalance;
                var transactions = await _context.Transaction
                    .Where(t => t.AccountId == account.AccountId && t.Date < startDate)
                    .ToListAsync();
                foreach (var transaction in transactions)
                {
                    balance += transaction.TransactionAmount;
                }
                accountBalances[account.AccountId] = balance;
            }

            return accountBalances;
        }
    }
}
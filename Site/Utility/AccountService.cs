using Microsoft.EntityFrameworkCore;
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
    }
}
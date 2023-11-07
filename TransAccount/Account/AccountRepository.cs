using Microsoft.EntityFrameworkCore;
using TransAccount.Database;
using TransAccount.User;

namespace TransAccount.Account
{
    /// <summary>
    /// The account repository.
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        private TransAccContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public AccountRepository(TransAccContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>The created account db.</returns>
        public async Task<DbAccount> CreateAccount(Account account)
        {
            DbAccount dbAccount = account.toDb();
            await this.context.Accounts.AddAsync(dbAccount);
            await this.context.SaveChangesAsync();
            return dbAccount;
        }

        /// <summary>
        /// Gets the account by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The desired account.</returns>
        public async Task<DbAccount?> GetAccountById(int id)
        {
            return await context.Accounts.Where(acc => acc.AccountID == id).SingleAsync();
        }

        /// <summary>
        /// Gets the accounts for user.
        /// </summary>
        /// <param name="customerId">The customer id.</param>
        /// <returns>A list of user dbAccounts.</returns
        public async Task<List<DbAccount>> GetAccountsForUser(int customerId)
        {
            return await this.context.Accounts.Where(acc => acc.CustomerId == customerId).OrderByDescending(acc => acc.AccountID).Include(acc => acc.User).Include(acc => acc.Transactions).ThenInclude(t => t.TransactionType).ToListAsync();        
        }

        /// <summary>
        /// Updates the account balance.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="balance">The balance.</param>
        /// <returns>A Task.</returns>
        public async Task UpdateAccountBalance(DbAccount account, int balance)
        {
            var accountModel = new Account(account);
            accountModel.SetBalance(balance);
            accountModel.UpdateDb(account);
            await this.context.SaveChangesAsync();
        }
    }
}

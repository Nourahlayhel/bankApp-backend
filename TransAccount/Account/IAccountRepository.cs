using TransAccount.Database;

namespace TransAccount.Account
{
    /// <summary>
    /// The account repository.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>The created account db.</returns>
        Task<DbAccount> CreateAccount(Account account);

        /// <summary>
        /// Gets the accounts for user.
        /// </summary>
        /// <param name="customerId">The customer id.</param>
        /// <returns>A list of user dbAccounts.</returns>
        Task<List<DbAccount>> GetAccountsForUser(int customerId);

        /// <summary>
        /// Gets the account by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The desired account.</returns>
        Task<DbAccount?> GetAccountById(int id);

        /// <summary>
        /// Updates the account balance.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="balance">The balance.</param>
        /// <returns>A Task.</returns>
        Task UpdateAccountBalance(DbAccount account, int balance);
    }
}

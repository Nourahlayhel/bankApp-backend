using TransAccount.Database;

namespace TransAccount.Account
{
    /// <summary>
    /// The account.
    /// </summary>
    public class Account
    {
        private readonly int accountId;
        private readonly int customerId;
        private readonly int currencyId;
        private int balance;
        private DateTime creationDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        /// <param name="accountId">The account id.</param>
        /// <param name="balance">The balance.</param>
        /// <param name="customerId">The customer id.</param>
        /// <param name="currencyId">The currency id.</param>
        /// <param name="creationDate">The creation date.</param>
        public Account(int accountId, int balance, int customerId, int currencyId, DateTime creationDate)
        {
            this.accountId = accountId;
            this.balance = balance;
            this.customerId = customerId;
            this.currencyId = currencyId;
            this.creationDate = creationDate;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Account"/> class.
        /// </summary>
        /// <param name="dbAccount">The db account.</param>
        public Account(DbAccount dbAccount) : this(dbAccount.AccountID, dbAccount.Balance, dbAccount.CustomerId, dbAccount.CurrencyId, dbAccount.CreationDate)
        {
        }

        /// <summary>
        /// Gets the balance.
        /// </summary>
        public int Balance => this.balance;

        /// <summary>
        /// Gets the customer id.
        /// </summary>
        public int CustomerId => this.customerId;

        /// <summary>
        /// Tos the dto model.
        /// </summary>
        /// <returns>An AccountDto.</returns>
        public virtual AccountDto ToDtoModel()
        {
            return new()
            {
                AccountId = accountId,
                Balance = balance,
                CustomerId = customerId,
                CreationDate = creationDate,
                CurrencyId = currencyId,
            };
        }
        public DbAccount toDb()
        {
            return new DbAccount()
            {
                AccountID = accountId,
                Balance = balance,
                CustomerId = customerId,
                CreationDate = creationDate,
                CurrencyId = currencyId,
            };
        }

        /// <summary>
        /// Sets the balance.
        /// </summary>
        /// <param name="balance">The balance.</param>
        public void SetBalance(int balance)
        {
            this.balance = balance;
        }

        /// <summary>
        /// Updates the db.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>A DbAccount.</returns>
        public virtual DbAccount UpdateDb(DbAccount account)
        {
            account.Balance = balance;
            return account;
        }
    }
}

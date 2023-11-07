using TransAccount.Database;

namespace TransAccount.Transactions
{
    /// <summary>
    /// The transaction.
    /// </summary>
    public class Transaction
    {
        private readonly int transactionId;
        private readonly int amount;
        private readonly int accountId;
        private readonly int transactionTypeId;
        private readonly DateTime transactionDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="accountId">The account id.</param>
        /// <param name="transactionTypeId">The transaction type id.</param>
        /// <param name="transactionDate">The transaction date.</param>
        public Transaction(int transactionId, int amount, int accountId, int transactionTypeId, DateTime transactionDate)
        {
            this.transactionId = transactionId;
            this.amount = amount;
            this.accountId = accountId;
            this.transactionTypeId = transactionTypeId;
            this.transactionDate = transactionDate;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Transaction"/> class.
        /// </summary>
        /// <param name="dbTransaction">The db transaction.</param>
        public Transaction(DbTransaction dbTransaction): this(dbTransaction.TransactionID, dbTransaction.Amount, dbTransaction.AccountID, dbTransaction.TypeID, dbTransaction.TransactionDate) 
        { }

        /// <summary>
        /// tos the db.
        /// </summary>
        /// <returns>A DbTransaction.</returns>
        public virtual DbTransaction toDb()
        {
            return new DbTransaction()
            {
                TransactionID = transactionId,
                AccountID = accountId,
                Amount = amount,
                TransactionDate = transactionDate,
                TypeID = transactionTypeId,
            };
        }
    }
}

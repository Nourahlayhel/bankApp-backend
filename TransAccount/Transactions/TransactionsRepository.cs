using Microsoft.EntityFrameworkCore;
using TransAccount.Database;
using TransAccount.TransactionType;

namespace TransAccount.Transactions
{
    /// <summary>
    /// The transactions repository.
    /// </summary>
    public class TransactionsRepository : ITransactionRepository
    {
        private TransAccContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TransactionsRepository(TransAccContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Executes the transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>A Task.</returns>
        public async Task ExecuteTransaction(Transaction transaction)
        {
            DbTransaction dbTransaction = transaction.toDb();
            await this.context.Transactions.AddAsync(dbTransaction);
            await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the transaction types.
        /// </summary>
        /// <returns>A List of transaction type models.</returns>
        public async Task<List<TransactionTypeModel>> GetTransactionTypes() 
        {
            var types = await this.context.TransactionTypes.ToListAsync();
            return types.Select(t => new TransactionTypeModel(t)).ToList();
        }

        /// <summary>
        /// Gets the transaction type id by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The transaction type id or null if it does not exist.</returns>
        public async Task<int?> GetTransactionTypeIdByName(Database.TransactionType name)
        {
            return await this.context.TransactionTypes.Where(t => t.Name == name).Select(t => t.TransactionTypeID).SingleOrDefaultAsync();
        }
    }
}

using TransAccount.TransactionType;

namespace TransAccount.Transactions
{
    /// <summary>
    /// The transaction repository.
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Executes the transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>A Task.</returns>
        Task ExecuteTransaction(Transaction transaction);

        /// <summary>
        /// Gets the transaction types.
        /// </summary>
        /// <returns>Transaction types models.</returns>
        Task<List<TransactionTypeModel>> GetTransactionTypes();

        /// <summary>
        /// Gets the transaction type id by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Transaction Type Id</returns>
        Task<int?> GetTransactionTypeIdByName(Database.TransactionType name);
    }
}

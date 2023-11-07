using TransAccount.Database;

namespace TransAccount.TransactionType
{
    /// <summary>
    /// The transaction type model.
    /// </summary>
    public class TransactionTypeModel
    {
        private readonly int transactionTypeId;
        private readonly string name;
        private readonly string? description;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionTypeModel"/> class.
        /// </summary>
        /// <param name="transactionTypeId">The transaction type id.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        public TransactionTypeModel(int transactionTypeId, string name, string description)
        {
            this.transactionTypeId = transactionTypeId;
            this.name = name;
            this.description = description;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionTypeModel"/> class.
        /// </summary>
        /// <param name="dbTransactionType">The db transaction type.</param>
        public TransactionTypeModel(DbTransactionType dbTransactionType) : this(dbTransactionType.TransactionTypeID, dbTransactionType.Name.ToString(), dbTransactionType.Description)
        { }

        /// <summary>
        /// tos the dto.
        /// </summary>
        /// <returns>A TransactionTypeDto.</returns>
        public TransactionTypeDto toDto()
        {
            return new TransactionTypeDto()
            {
                TransactionTypeId = this.transactionTypeId,
                Name = this.name,
                Description = this.description,
            };
        }
    }
}

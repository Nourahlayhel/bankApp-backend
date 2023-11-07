namespace TransAccount.Transactions
{
    /// <summary>
    /// The transaction dto.
    /// </summary>
    public class TransactionDto
    {
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// Gets or sets the transaction type id.
        /// </summary>
        public int TransactionTypeId { get; set; }
        /// <summary>
        /// Gets or sets the transaction type.
        /// </summary>
        public string TransactionType { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the account id.
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// Gets or sets the transaction date.
        /// </summary>
        public DateTime TransactionDate { get; set; } 
    }
}

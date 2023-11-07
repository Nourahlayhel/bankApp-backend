namespace TransAccount.TransactionType
{
    /// <summary>
    /// The transaction type dto.
    /// </summary>
    public class TransactionTypeDto
    {
        /// <summary>
        /// Gets or sets the transaction type id.
        /// </summary>
        public int TransactionTypeId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string? Description { get; set; }
    }
}

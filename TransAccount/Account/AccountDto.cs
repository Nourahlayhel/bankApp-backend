using TransAccount.Transactions;
using TransAccount.User;

namespace TransAccount.Account
{
    /// <summary>
    /// The account dto.
    /// </summary>
    public class AccountDto
    {
        /// <summary>
        /// Gets or sets the account id.
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the balance.
        /// </summary>
        public int Balance { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the currency id.
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Gets or sets the transactions.
        /// </summary>
        public List<TransactionDto>? Transactions { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public UserDto? User { get; set; }
    }
}

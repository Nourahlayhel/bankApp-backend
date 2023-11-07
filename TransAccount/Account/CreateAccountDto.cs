namespace TransAccount.Account
{
    /// <summary>
    /// The create account dto.
    /// </summary>
    public class CreateAccountDto
    {
        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the initial credit.
        /// </summary>
        public int InitialCredit { get; set; }

        /// <summary>
        /// Gets or sets the currency id.
        /// </summary>
        public int CurrencyId { get; set; }
    }
}

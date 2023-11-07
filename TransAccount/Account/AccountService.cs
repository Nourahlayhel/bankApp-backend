using TransAccount.Database;
using TransAccount.Transactions;
using TransAccount.User;

namespace TransAccount.Account
{
    /// <summary>
    /// The account service.
    /// </summary>
    public class AccountService
    {
        private IAccountRepository accountRepository;
        private TransactionsService transactionsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="transactionsService">The transactions service.</param>
        public AccountService(IAccountRepository accountRepository, TransactionsService transactionsService)
        {
            this.accountRepository = accountRepository;
            this.transactionsService = transactionsService;
        }

        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <param name="createAccountDto">The create account dto.</param>
        /// <returns>The created account dto.</returns>
        public async Task<AccountDto> CreateAccount(CreateAccountDto createAccountDto)
        {
            Account account = new (0, 0, createAccountDto.CustomerId, createAccountDto.CurrencyId, DateTime.Now);
            DbAccount createdAccount = await this.accountRepository.CreateAccount(account);
            AccountDto accountDto = new Account(createdAccount).ToDtoModel();
            accountDto.Balance = createAccountDto.InitialCredit;
            if(createAccountDto.InitialCredit != 0)
            {
                var transaction = await this.transactionsService.AddDepositTransactionToNewAccount(createdAccount, createAccountDto.InitialCredit);
                accountDto.Transactions = new() { transaction };
            }
            return accountDto;
        }

        /// <summary>
        /// Gets the accounts for user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A list of user accounts.</returns>
        public async Task<List<AccountDto>> GetAccountsForUser(int userId)
        {
            var accounts = await this.accountRepository.GetAccountsForUser(userId);
            return accounts.Select(acc => new AccountDto()
            {
                AccountId = acc.AccountID,
                Transactions = acc.Transactions.Select(t => new Transactions.TransactionDto() { Amount = t.Amount, TransactionType = t.TransactionType.Name.ToString(), TransactionDate = t.TransactionDate }).ToList(),
                User = new UserModel(acc.User).toDtoModel(),
                CurrencyId = acc.CurrencyId,
                Balance = acc.Balance,
            }).ToList();

        }
    }
}

using TransAccount.Account;
using TransAccount.Database;
using TransAccount.TransactionType;

namespace TransAccount.Transactions
{
    /// <summary>
    /// The transactions service.
    /// </summary>
    public class TransactionsService
    {
        private ITransactionRepository transactionsRepository;
        private IAccountRepository accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsService"/> class.
        /// </summary>
        /// <param name="transacrionsRepository">The transacrions repository.</param>
        /// <param name="accountRepository">The account repository.</param>
        public TransactionsService(ITransactionRepository transacrionsRepository, IAccountRepository accountRepository)
        {
            this.transactionsRepository = transacrionsRepository;
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Adds the deposit transaction to new account.
        /// </summary>
        /// <param name="createdAccount">The created account.</param>
        /// <param name="deposit">The deposit.</param>
        /// <returns>The added transactionDto.</returns>
        public async Task<TransactionDto> AddDepositTransactionToNewAccount(DbAccount createdAccount, int deposit)
        {
            TransactionDto transactionDto = new()
            {
                AccountId = createdAccount.AccountID,
                Amount = deposit,
                TransactionType = Database.TransactionType.Deposit.ToString(),
                TransactionTypeId = await this.transactionsRepository.GetTransactionTypeIdByName(Database.TransactionType.Deposit) ?? throw new Exception(),
                TransactionDate = DateTime.Now,
            };
            await this.ExecuteTransactionOnAccount(createdAccount, transactionDto);
            return transactionDto;
        }
        /// <summary>
        /// Executes the transaction.
        /// </summary>
        /// <param name="transactionDto">The transaction dto.</param>
        /// <returns>A Task.</returns>
        public async Task ExecuteTransaction(TransactionDto transactionDto)
        {
            var account = await this.accountRepository.GetAccountById(transactionDto.AccountId) ?? throw new Exception();
            transactionDto.TransactionDate = DateTime.Now;
            await this.ExecuteTransactionOnAccount(account, transactionDto);
            
        }

        /// <summary>
        /// Executes the transaction on account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="transactionDto">The transaction dto.</param>
        /// <returns>A Task.</returns>
        public async Task ExecuteTransactionOnAccount(DbAccount account, TransactionDto transactionDto)
        {
            Transaction trans = new(0, transactionDto.Amount, transactionDto.AccountId, transactionDto.TransactionTypeId, transactionDto.TransactionDate);
            await this.transactionsRepository.ExecuteTransaction(trans);
            var currentBalance = account.Balance;
            if(transactionDto.TransactionType == Database.TransactionType.Deposit.ToString())
            {
                currentBalance += transactionDto.Amount;
            }
            else
            {
                if(currentBalance < transactionDto.Amount)
                {
                    throw new Exception();
                }

                currentBalance -= transactionDto.Amount;
            }
            await this.accountRepository.UpdateAccountBalance(account, currentBalance);
        }
        /// <summary>
        /// Gets the transaction types.
        /// </summary>
        /// <returns>A list of transactionType dtos.</returns>
        public async Task<List<TransactionTypeDto>> GetTransactionTypes()
        {
            return (await this.transactionsRepository.GetTransactionTypes()).Select(t => t.toDto()).ToList();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransAccount.TransactionType;

namespace TransAccount.Transactions
{
    /// <summary>
    /// The transactions controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private TransactionsService transactionsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsController"/> class.
        /// </summary>
        /// <param name="transactionsService">The transactions service.</param>
        public TransactionsController(TransactionsService transactionsService)
        {
            this.transactionsService = transactionsService;
        }

        /// <summary>
        /// Gets the transaction types.
        /// </summary>
        /// <returns>A List of transaction type dto.</returns>
        [HttpGet("types")]
        public async Task<ActionResult<List<TransactionTypeDto>>> GetTransactionTypes()
        {
            var types = await this.transactionsService.GetTransactionTypes();
            return this.Ok(types);
        }

        /// <summary>
        /// Executes the transaction.
        /// </summary>
        /// <param name="transactionDto">The transaction dto.</param>
        /// <returns>A Task.</returns>
        [HttpPost]
        public async Task<IActionResult> ExecuteTransaction(TransactionDto transactionDto)
        {
            await this.transactionsService.ExecuteTransaction(transactionDto);
            return this.Ok();
        }
    }
}

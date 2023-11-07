using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TransAccount.Account
{
    /// <summary>
    /// The account controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private AccountService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public AccountController(AccountService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Gets the accounts of user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>accounts of user.</returns>
        [HttpGet("{userId}")]
        public async Task<ActionResult<List<AccountDto>>> GetAccountsOfUser(int userId)
        {
            var accounts = await this.service.GetAccountsForUser(userId);
            return this.Ok(accounts);
        }

        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <param name="createAccountDto">The create account dto.</param>
        /// <returns>The newly created account.</returns>
        [HttpPost]
        public async Task<ActionResult<AccountDto>> CreateAccount(CreateAccountDto createAccountDto)
        {
            AccountDto account = await this.service.CreateAccount(createAccountDto);
            return this.Ok(account);
        }
    }
}

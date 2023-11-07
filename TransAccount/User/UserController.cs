using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TransAccount.User
{
    /// <summary>
    /// The user controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Authenticates the.
        /// </summary>
        /// <param name="loginDto">The login dto.</param>
        /// <returns>The user dto or unauthorized if the credentials are wrong.</returns>
        [HttpPost("Authenticate")]
        public async Task<ActionResult<UserDto>> Authenticate(LoginDto loginDto)
        {
            var user = await this.userService.Authenticate(loginDto);
            if(user == null)
            {
                return this.Unauthorized();
            }
            return this.Ok(user);
        }
    }
}

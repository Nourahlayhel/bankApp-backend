using Microsoft.EntityFrameworkCore;
using TransAccount.Database;

namespace TransAccount.User
{
    /// <summary>
    /// The user repository.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly TransAccContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UserRepository(TransAccContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Authenticates the.
        /// </summary>
        /// <param name="loginDto">The login dto.</param>
        /// <returns>The user model or null if the credentials are wrong.</returns>
        public async Task<UserModel?> Authenticate(LoginDto loginDto)
        {
            var user = await this.context.Users.Where(u => u.Email == loginDto.Email).SingleOrDefaultAsync();
            if(user == null)
            {
                return null;
            }
            else
            {
                if(user.Password == loginDto.Password)
                {
                    return new UserModel(user);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}

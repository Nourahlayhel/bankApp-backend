namespace TransAccount.User
{
    /// <summary>
    /// The user service.
    /// </summary>
    public class UserService
    {
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        /// <summary>
        /// Authenticates the.
        /// </summary>
        /// <param name="loginDto">The login dto.</param>
        /// <returns>The userDto.</returns>
        public async Task<UserDto?> Authenticate(LoginDto loginDto)
        {
            var user = await this.userRepository.Authenticate(loginDto);
            return user?.toDtoModel();
        }
    }
}

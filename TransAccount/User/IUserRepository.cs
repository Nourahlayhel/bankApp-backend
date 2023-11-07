namespace TransAccount.User
{
    /// <summary>
    /// The user repository.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Authenticates the.
        /// </summary>
        /// <param name="loginDto">The login dto.</param>
        /// <returns>The user model or null if the credentials are wrong.</returns>
        Task<UserModel?> Authenticate(LoginDto loginDto);
    }
}

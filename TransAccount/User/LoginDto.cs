
namespace TransAccount.User
{
    /// <summary>
    /// The login dto.
    /// </summary>
    public class LoginDto 
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}

using TransAccount.Database;

namespace TransAccount.User
{
    /// <summary>
    /// The user model.
    /// </summary>
    public class UserModel
    {
        private readonly int userId;
        private readonly string firstName;
        private readonly string lastName;
        private readonly string email;
        private readonly string password;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserModel"/> class.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        public UserModel(int userId, string firstName, string lastName, string email, string password)
        {
            this.userId = userId;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.password = password;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserModel"/> class.
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        public UserModel(DbUser dbUser) : this(dbUser.UserID, dbUser.FirstName, dbUser.LastName, dbUser.Email, dbUser.Password)
        {
        }

        /// <summary>
        /// tos the dto model.
        /// </summary>
        /// <returns>An UserDto.</returns>
        public UserDto toDtoModel()
        {
            return new UserDto()
            {
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
            };
        }
    }
}

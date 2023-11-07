using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransAccount.Database
{
    [Table("User")]
    public class DbUser
    {
        public DbUser()
        {
            Accounts = new HashSet<DbAccount>();
        }

        [Key]
        public int UserID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public virtual ICollection<DbAccount> Accounts { get; set; }

    }
}

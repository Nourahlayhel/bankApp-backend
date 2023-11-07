using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransAccount.Database
{
    [Table("Account")]
    public class DbAccount
    {
        public DbAccount()
        {
            Transactions = new HashSet<DbTransaction>();
        }

        [Key]
        public int AccountID { get; set; }
        public int CustomerId { get; set; }
        public int Balance { get; set; }
        public DateTime CreationDate { get; set; }
        public int CurrencyId { get; set; }
        public virtual ICollection<DbTransaction> Transactions { get; set; }

#nullable disable

        [ForeignKey("CustomerId")]
        [InverseProperty("Accounts")]
        public virtual DbUser User { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual DbCurrency Currency { get; set; }
    }
}

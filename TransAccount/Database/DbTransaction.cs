using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransAccount.Database
{
    [Table("Transaction")]
    public class DbTransaction
    {
        [Key]
        public int TransactionID { get; set; }
        public int TypeID { get; set; }
        public int AccountID { get; set; }
        public int Amount { get; set; }
        public DateTime TransactionDate { get; set; }

#nullable disable
        [ForeignKey("AccountID")]
        [InverseProperty("Transactions")]
        public virtual DbAccount Account { get; set; }

        [ForeignKey("TypeID")]
        public virtual DbTransactionType TransactionType { get; set; }
    }
}

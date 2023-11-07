using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransAccount.Database
{
    [Table("TransactionType")]
    public class DbTransactionType
    {
        [Key]
        public int TransactionTypeID { get; set; }
        public TransactionType Name { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<DbTransaction>? Transactions { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransAccount.Database
{
    [Table("Currency")]
    public class DbCurrency
    {
        [Key]
        public int CurrencyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public char Symbol { get; set; }
    }
}

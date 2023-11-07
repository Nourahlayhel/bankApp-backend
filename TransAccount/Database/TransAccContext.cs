using Microsoft.EntityFrameworkCore;

namespace TransAccount.Database
{
    public partial class TransAccContext: DbContext
    {
        public TransAccContext(DbContextOptions<TransAccContext> options) : base(options)
        {
        }

#nullable disable
        public virtual DbSet<DbUser> Users { get; set; }
        public virtual DbSet<DbAccount> Accounts { get; set; }
        public virtual DbSet<DbTransaction> Transactions { get; set; }
        public virtual DbSet<DbTransactionType> TransactionTypes { get; set; }
        public virtual DbSet<DbCurrency> Currencies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbAccount>()
             .HasOne(o => o.User)
             .WithMany(t => t.Accounts)
             .HasForeignKey(o => o.CustomerId);


            modelBuilder.Entity<DbTransaction>()
            .HasOne(c => c.Account)
            .WithMany(o => o.Transactions)
            .HasForeignKey(c => c.AccountID);

            modelBuilder.Entity<DbTransaction>()
                .HasOne(c => c.TransactionType)
                .WithMany(o => o.Transactions)
                .HasForeignKey(c => c.TypeID);

            modelBuilder.Entity<DbTransactionType>(entity =>
            {
                entity.Property(f => f.Name)
                .HasConversion(
                    v => v.ToString(),
                    v => (TransactionType)Enum.Parse(typeof(TransactionType), v));
            });
            this.OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

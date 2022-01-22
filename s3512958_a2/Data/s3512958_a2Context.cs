using Microsoft.EntityFrameworkCore;
using s3512958_a2.Models;
namespace s3512958_a2.Data
{
	public class s3512958_a2Context: DbContext
	{
		public s3512958_a2Context(DbContextOptions<s3512958_a2Context> options) : base(options)
		{ }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Set check constraints (cannot be expressed with data annotations).
            builder.Entity<Login>().HasCheckConstraint("CH_Login_LoginID", "len(LoginID) = 8").
                HasCheckConstraint("CH_Login_PasswordHash", "len(PasswordHash) = 64");
            builder.Entity<Account>().HasCheckConstraint("CH_Account_Balance", "Balance >= 0");
            builder.Entity<Transaction>().HasCheckConstraint("CH_Transaction_Amount", "Amount > 0");

            // Configure ambiguous Account.Transactions navigation property relationship.
            builder.Entity<Transaction>().
                HasOne(x => x.Account).WithMany(x => x.Transactions).HasForeignKey(x => x.AccountNumber);
        }
        public DbSet<Login> Login { get; set; }
		public DbSet<Customer> Customer { get; set; }
		public DbSet<Account> Account { get; set; }
		public DbSet<Transaction> Transaction { get; set; }
	}
}


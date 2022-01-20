using Microsoft.EntityFrameworkCore;
using s3512958_a2.Models;
namespace s3512958_a2.Data
{
	public class s3512958_a2Context: DbContext
	{
		public s3512958_a2Context(DbContextOptions<s3512958_a2Context> options) : base(options)
		{ }

		public DbSet<Login> Login { get; set; }
		public DbSet<Customer> Customer { get; set; }
	}
}


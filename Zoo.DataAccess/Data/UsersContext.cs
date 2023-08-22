using Zoo.Models;

namespace Zoo.DataAccess.Data;

public class UsersContext : IdentityDbContext<IdentityUser>
{
	public UsersContext(DbContextOptions<UsersContext> options) : base(options)
	{
	}
	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
	}
}

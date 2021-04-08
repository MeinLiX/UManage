using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UManager.Models
{
    public class IdentityCTX: IdentityDbContext<UserModel>
    {
        public IdentityCTX(DbContextOptions<IdentityCTX> options):base(options)
        {
            Database.EnsureCreated();
        }
    }
}

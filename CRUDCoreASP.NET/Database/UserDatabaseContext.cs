using CRUDCoreASP.NET.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDCoreASP.NET.Database
{
    public class UserDatabaseContext: DbContext
    {
        public UserDatabaseContext(DbContextOptions<UserDatabaseContext> options) : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
    }
}

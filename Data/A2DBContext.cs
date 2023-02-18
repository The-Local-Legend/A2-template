using Microsoft.EntityFrameworkCore;
using A2.Models;
namespace A2.Data
{
    public class A2DBContext: DbContext
    {
        public A2DBContext(DbContextOptions<A2DBContext> options) : base(options) { }
        public DbSet<GameRecord> Gamerecords { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
namespace APINTUIT.Models
{
    public class DBintuitContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }

        public DBintuitContext(DbContextOptions<DBintuitContext> options) : base(options)
        {
        }
    }
}

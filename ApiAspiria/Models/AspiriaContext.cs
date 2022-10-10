using Microsoft.EntityFrameworkCore;

namespace ApiAspiria.Models
{
    public class AspiriaContext : DbContext
    {
        public AspiriaContext(DbContextOptions<AspiriaContext> option)
            : base(option)
        {

        }

        public DbSet<Juguete> juguetes { get; set; }

    }
}

using Microsoft.EntityFrameworkCore;

namespace MvcHtmx.Models
{
    public class MvcHtmxContext : DbContext
    {
        public MvcHtmxContext(DbContextOptions<MvcHtmxContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
    }
}

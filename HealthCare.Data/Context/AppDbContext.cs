using Microsoft.EntityFrameworkCore;

namespace HealthCare.Data.Context
{
    class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options);
}

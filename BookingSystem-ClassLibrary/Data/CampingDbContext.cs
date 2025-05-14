using Microsoft.EntityFrameworkCore;

namespace BookingSystem_ClassLibrary.Data
{
    public class CampingDbContext : DbContext
    {
        public CampingDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Book_WebAPI.Data
{
    public class BookAuthDbContext: IdentityDbContext
    {
        public BookAuthDbContext(DbContextOptions<BookAuthDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var readerRoleId = "004c7e80 - 7dfc - 44be - 8952 - 2c7130898655";
            var writeRoleId = "71e282d3-76ca-485e-b094-eff019287fa5";
            base.OnModelCreating(builder);
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name ="Reader",
                    NormalizedName="Reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = writeRoleId,
                    ConcurrencyStamp = writeRoleId,
                    Name ="Writter",
                    NormalizedName="Writter".ToUpper()
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

        }

    }
}

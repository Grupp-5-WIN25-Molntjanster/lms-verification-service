using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VerificationService.Data;

namespace VerificationService.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseSqlServer(
            "Server=YOUR_SERVER;Database=YOUR_DB;Trusted_Connection=True;TrustServerCertificate=True"
        );

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend;

//mirgration commad:
//dotnet ef migrations add InitialCreate --project StudentScheduleBackend --startup-project StudentScheduleClient
internal class AppDbContextFactory : IDesignTimeDbContextFactory<Context>
{
    public Context CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<Context>();

        // Use your actual connection string or a placeholder
        optionsBuilder.UseSqlServer("Server=localhost;Database=StudentSchedule;Trusted_Connection=True;TrustServerCertificate=True;");

        return new Context(optionsBuilder.Options);
    }
}


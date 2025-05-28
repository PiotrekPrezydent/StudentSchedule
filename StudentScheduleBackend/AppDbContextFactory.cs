using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend;

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


using StudentScheduleBackend;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Extensions;
using StudentScheduleBackend.Repositories;

namespace CLI
{
    internal class CLI
    {
        static void Main(string[] args)
        {
            string s = Context.BuildConnectionString("./AppSettings.json", "ADMIN", "ADMIN", out int accountId);

            Context c = Context.Initialize(s);
            Exporter.CreateBackups(c, "./Backups");
            Console.WriteLine("ended backups");
        }
    }
}

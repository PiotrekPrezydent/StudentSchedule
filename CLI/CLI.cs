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

            Repository<Account> ac = new(c);
        }
    }
}

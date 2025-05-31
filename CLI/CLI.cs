using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using StudentScheduleBackend;
using StudentScheduleBackend.Entities;
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

            foreach(var a in ac.GetAll())
            {
                Console.WriteLine(a.ToString());
            }
        }
    }
}

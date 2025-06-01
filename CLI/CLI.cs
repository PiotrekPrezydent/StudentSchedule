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
            List<Entity> entities = ac.GetAll().Cast<Entity>().ToList();

            foreach(var a in entities.PanDa5ZaTenSuperFilter(new() { new("Id",1)}))
            {
                Console.WriteLine(a.GetColumnsWithValues().ElementAt(0).Key + " --- " + a.GetColumnsWithValues().ElementAt(0).Value);
            }
        }
    }
}

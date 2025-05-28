using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Repositories;

namespace CLI
{
    internal class CLI
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world");

            Context c = Context.Initialize(null);
            var key = c.Model.FindEntityType(typeof(Subject)).FindPrimaryKey();
            Console.WriteLine(key);

            Console.Read();
        }
    }
}

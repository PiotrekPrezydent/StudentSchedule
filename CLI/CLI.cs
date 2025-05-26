using StudentScheduleBackend;
using StudentScheduleBackend.Repositories;

namespace CLI
{
    internal class CLI
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world");

            Context c = Context.Initialize(null);
            AccountRepository ac = new(c);;
            var a = ac.GetAll();
            var st = a[0];

            StudentProgramRepository spr = new(c);
            var programs = spr.GetAll().Where(e => e.StudentId == st.Id).Select(e => e.Program);

            ClassRepository cr = new(c);

            var cl = cr.GetAll().Where(e => programs.Any(p => p.Id == e.ProgramId));
            Console.WriteLine($"student {st} ma zajęcia:");

            foreach(var cla in cl)
            {
                Console.WriteLine(cla);
            }

            Console.Read();
        }
    }
}

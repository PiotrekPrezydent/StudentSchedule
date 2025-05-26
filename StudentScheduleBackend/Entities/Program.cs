using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentScheduleBackend.Entities
{
    public class Program
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<StudentProgram> StudentPrograms { get; set; }
        public ICollection<Class> Classes { get; set; }
    }

}

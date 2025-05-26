using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentScheduleBackend.Entities
{
    public class StudentProgram
    {
        public int StudentId { get; set; }
        public int ProgramId { get; set; }

        public Student Student { get; set; }
        public Program Program { get; set; }
    }
}

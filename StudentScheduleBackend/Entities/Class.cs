using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentScheduleBackend.Entities
{
    public class Class
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public int SubjectId { get; set; }
        public int Year { get; set; }
        public string Weekday { get; set; }
        public int? ClassroomId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public Program Program { get; set; }
        public Subject Subject { get; set; }
        public Classroom Classroom { get; set; }
    }
}

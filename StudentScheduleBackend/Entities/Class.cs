using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using StudentScheduleBackend.Attributes;

namespace StudentScheduleBackend.Entities
{
    [Table("Classes")]
    public class Class : Entity
    {
        [ForeignKeyOf(typeof(Program))]
        public int ProgramId { get; set; }

        [ForeignKeyOf(typeof(Subject))]
        public int SubjectId { get; set; }

        public int Year { get; set; }
        public string Weekday { get; set; }

        [ForeignKeyOf(typeof(Classroom))]
        public int ClassroomId { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        [JsonIgnore]
        public Program? Program { get; set; }

        [JsonIgnore]
        public Subject? Subject { get; set; }

        [JsonIgnore]
        public Classroom? Classroom { get; set; }

        public Class(int id, int programId, int subjectId, int year, string weekday, int classroomId, TimeSpan startTime, TimeSpan endTime)
        {
            Id = id;
            ProgramId = programId;
            SubjectId = subjectId;
            Year = year;
            Weekday = weekday;
            ClassroomId = classroomId;
            StartTime = startTime;
            EndTime = endTime;
        }

        public Class(int programId, int subjectId, int year, string weekday, int classroomId, TimeSpan startTime, TimeSpan endTime)
        {
            ProgramId = programId;
            SubjectId = subjectId;
            Year = year;
            Weekday = weekday;
            ClassroomId = classroomId;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}

using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public Program Program { get; set; }

        [JsonIgnore]
        public Subject Subject { get; set; }

        [JsonIgnore]
        public Classroom Classroom { get; set; }

        //EF constructor
        public Class() { }

        public Class(int id, int programId, int subjectId, int year, string weekday, int? classroomId, TimeSpan? startTime, TimeSpan? endTime)
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

        public Class(int programId, int subjectId, int year, string weekday, int? classroomId, TimeSpan? startTime, TimeSpan? endTime)
        {
            ProgramId = programId;
            SubjectId = subjectId;
            Year = year;
            Weekday = weekday;
            ClassroomId = classroomId;
            StartTime = startTime;
            EndTime = endTime;
        }

        public override string ToString() => $"{Id}\t{ProgramId}\t{SubjectId}\t{Year}\t{Weekday}\t{ClassroomId}\t{StartTime}\t{EndTime}";
    }
}

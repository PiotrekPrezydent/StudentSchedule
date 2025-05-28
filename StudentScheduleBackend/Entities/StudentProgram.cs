using System.Text.Json.Serialization;

namespace StudentScheduleBackend.Entities
{
    public class StudentProgram
    {
        public int StudentId { get; set; }
        public int ProgramId { get; set; }

        [JsonIgnore]
        public Student Student { get; set; }

        [JsonIgnore]
        public Program Program { get; set; }


        //EF constructor
        public StudentProgram() { }

        public StudentProgram(int studentId, int programId)
        {
            StudentId = studentId;
            ProgramId = programId;
        }

        public override string ToString() => $"{StudentId}\t{ProgramId}";
    }
}

namespace StudentScheduleBackend.Entities
{
    public class StudentProgram
    {
        public int StudentId { get; set; }
        public int ProgramId { get; set; }

        public Student Student { get; set; }
        public Program Program { get; set; }

        public override string ToString() => $"{StudentId}\t{ProgramId}";
    }
}

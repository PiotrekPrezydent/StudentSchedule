namespace StudentScheduleBackend.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }

        public Account Account { get; set; }

        public ICollection<StudentProgram> StudentPrograms { get; set; }

        public override string ToString() => $"{Id}\t{FirstName}\t{LastName}\t{IndexNumber}";
    }
}

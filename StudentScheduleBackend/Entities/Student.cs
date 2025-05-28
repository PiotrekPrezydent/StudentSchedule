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

        public Student() { }

        public Student(int id, string firstName, string lastName, string indexNumber)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            IndexNumber = indexNumber;
        }

        public Student(string firstName, string lastName, string indexNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            IndexNumber = indexNumber;
        }

        public override string ToString() => $"{Id}\t{FirstName}\t{LastName}\t{IndexNumber}";
    }
}

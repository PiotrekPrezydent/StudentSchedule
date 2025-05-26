namespace StudentScheduleBackend.Entities
{
    public class Program
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<StudentProgram> StudentPrograms { get; set; }
        public ICollection<Class> Classes { get; set; }

        public override string ToString() => $"{Id}\t{Name}";
    }

}

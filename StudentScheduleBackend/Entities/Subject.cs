namespace StudentScheduleBackend.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Class> Classes { get; set; }

        public Subject() { }

        public Subject(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => $"{Id}\t{Name}";
    }
}

namespace StudentScheduleBackend.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Class> Classes { get; set; }

        public override string ToString() => $"{Id}\t{Name}";
    }
}

using System.Text.Json.Serialization;

namespace StudentScheduleBackend.Entities
{
    public class Program
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<StudentProgram> StudentPrograms { get; set; }

        [JsonIgnore]
        public ICollection<Class> Classes { get; set; }

        public Program() { }

        public Program(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Program(string name)
        {
            Name = name;
        }

        public override string ToString() => $"{Id}\t{Name}";
    }

}

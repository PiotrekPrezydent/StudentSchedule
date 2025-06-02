using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StudentScheduleBackend.Entities
{
    [Table("Programs")]
    public class Program : Entity
    {
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<StudentProgram>? StudentPrograms { get; set; }

        [JsonIgnore]
        public ICollection<Class>? Classes { get; set; }

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
    }
}

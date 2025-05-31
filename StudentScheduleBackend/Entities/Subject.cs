using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StudentScheduleBackend.Entities
{
    [Table("Subjects")]
    public class Subject : Entity
    {
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Class>? Classes { get; set; }

        public Subject(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Subject(string name)
        {
            Name = name;
        }
    }
}
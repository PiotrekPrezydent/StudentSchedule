using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StudentScheduleBackend.Entities
{
    [Table("Classrooms")]
    public class Classroom : Entity
    {
        public string Building { get; set; }
        public string RoomNumber { get; set; }

        [JsonIgnore]
        public ICollection<Class>? Classes { get; set; }

        public Classroom(int id, string building, string roomNumber)
        {
            Id = id;
            Building = building;
            RoomNumber = roomNumber;
        }

        public Classroom(string building, string roomNumber)
        {
            Building = building;
            RoomNumber = roomNumber;
        }
    }
}
using System.Text.Json.Serialization;

namespace StudentScheduleBackend.Entities
{
    public class Classroom
    {
        public int Id { get; set; }
        public string Building { get; set; }
        public string RoomNumber { get; set; }

        [JsonIgnore]
        public ICollection<Class> Classes { get; set; }

        public Classroom() { }

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

        public override string ToString() => $"{Id}\t{Building}\t{RoomNumber}";
    }
}

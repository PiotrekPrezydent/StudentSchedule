namespace StudentScheduleBackend.Entities
{
    public class Classroom
    {
        public int Id { get; set; }
        public string Building { get; set; }
        public string RoomNumber { get; set; }

        public ICollection<Class> Classes { get; set; }

        public Classroom() { }

        public Classroom(string building, string roomNumber)
        {
            Building = building;
            RoomNumber = roomNumber;
        }

        public override string ToString() => $"{Id}\t{Building}\t{RoomNumber}";
    }
}

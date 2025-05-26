namespace StudentScheduleBackend.Entities
{
    public class Classroom
    {
        public int Id { get; set; }
        public string Building { get; set; }
        public string RoomNumber { get; set; }

        public ICollection<Class> Classes { get; set; }

        public override string ToString() => $"{Id}\t{Building}\t{RoomNumber}";
    }
}

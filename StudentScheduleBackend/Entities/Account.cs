namespace StudentScheduleBackend.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public Student Student { get; set; }

        public override string ToString() => $"{Id}\t{StudentId}\t{Login}\t{Password}";
    }

}

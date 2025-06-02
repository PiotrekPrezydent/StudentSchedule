using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using StudentScheduleBackend.Attributes;

namespace StudentScheduleBackend.Entities
{
    [Table("Students")]
    public class Student : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }

        [ForeignKeyOf(typeof(Account))]
        public int AccountId { get; set; }

        [JsonIgnore]
        public Account? Account { get; set; }

        [JsonIgnore]
        public ICollection<StudentProgram>? StudentPrograms { get; set; }

        public Student() { }

        public Student(int id, string firstName, string lastName, string indexNumber, int accountId)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            IndexNumber = indexNumber;
            AccountId = accountId;
        }

        public Student(string firstName, string lastName, string indexNumber, int accountId)
        {
            FirstName = firstName;
            LastName = lastName;
            IndexNumber = indexNumber;
            AccountId = accountId;
        }
    }
}
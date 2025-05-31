using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StudentScheduleBackend.Entities
{
    public class Student : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }

        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }

        [JsonIgnore]
        public Account? Account { get; set; }

        [JsonIgnore]
        public ICollection<StudentProgram>? StudentPrograms { get; set; }

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
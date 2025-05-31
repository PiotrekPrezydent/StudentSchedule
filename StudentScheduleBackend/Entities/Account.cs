using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StudentScheduleBackend.Entities
{
    [Table("Accounts")]
    public class Account : Entity
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        [JsonIgnore]
        public Student? Student { get; set; }

        public Account(int id, string login, string password, bool isAdmin)
        {
            Id = id;
            Login = login;
            Password = password;
            IsAdmin = isAdmin;
        }

        public Account(string login, string password, bool isAdmin)
        {
            Login = login;
            Password = password;
            IsAdmin = isAdmin;
        }
    }

}

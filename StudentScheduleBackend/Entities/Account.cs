using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentScheduleBackend.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public Student Student { get; set; }
    }

}

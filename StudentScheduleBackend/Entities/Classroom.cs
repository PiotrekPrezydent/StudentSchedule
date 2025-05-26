using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentScheduleBackend.Entities
{
    public class Classroom
    {
        public int Id { get; set; }
        public string Building { get; set; }
        public string RoomNumber { get; set; }

        public ICollection<Class> Classes { get; set; }
    }
}

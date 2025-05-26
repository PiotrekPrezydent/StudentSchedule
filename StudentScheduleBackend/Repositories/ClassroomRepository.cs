using System;
using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Entities;

namespace StudentScheduleBackend.Repositories
{
    public class ClassroomRepository
    {
        readonly Context _context;
        public ClassroomRepository(Context context) => _context = context;

        public bool Add(Classroom classroom)
        {
            _context.Classrooms.Add(classroom);
            return _context.SaveChanges() > 0;
        }

        public List<Classroom> GetAll() => _context.Classrooms.Include(c => c.Classes).ToList();

        public Classroom? GetById(int id) => _context.Classrooms.Include(c => c.Classes).FirstOrDefault(c => c.Id == id);

        public bool Update(Classroom classroom)
        {
            _context.Classrooms.Update(classroom);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var classroom = _context.Classrooms.Find(id);
            if (classroom == null) return false;
            _context.Classrooms.Remove(classroom);
            return _context.SaveChanges() > 0;
        }
    }
}

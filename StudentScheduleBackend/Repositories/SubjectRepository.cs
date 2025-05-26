using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Entities;

namespace StudentScheduleBackend.Repositories
{
    public class SubjectRepository
    {
        readonly Context _context;
        public SubjectRepository(Context context) => _context = context;

        public bool Add(Subject subject)
        {
            _context.Subjects.Add(subject);
            return _context.SaveChanges() > 0;
        }

        public List<Subject> GetAll() => _context.Subjects.Include(s => s.Classes).ToList();

        public Subject? GetById(int id) => _context.Subjects.Include(s => s.Classes).FirstOrDefault(s => s.Id == id);

        public bool Update(Subject subject)
        {
            _context.Subjects.Update(subject);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var subject = _context.Subjects.Find(id);
            if (subject == null) return false;
            _context.Subjects.Remove(subject);
            return _context.SaveChanges() > 0;
        }
    }
}

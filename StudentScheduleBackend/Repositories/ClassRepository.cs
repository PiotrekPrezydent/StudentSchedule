using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Entities;

namespace StudentScheduleBackend.Repositories
{
    public class ClassRepository
    {
        readonly Context _context;
        public ClassRepository(Context context) => _context = context;

        public bool Add(Class @class)
        {
            _context.Classes.Add(@class);
            return _context.SaveChanges() > 0;
        }

        public List<Class> GetAll() => _context.Classes
            .Include(c => c.Program)
            .Include(c => c.Subject)
            .Include(c => c.Classroom)
            .ToList();

        public Class? GetById(int id) => _context.Classes
            .Include(c => c.Program)
            .Include(c => c.Subject)
            .Include(c => c.Classroom)
            .FirstOrDefault(c => c.Id == id);

        public bool Update(Class @class)
        {
            _context.Classes.Update(@class);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var @class = _context.Classes.Find(id);
            if (@class == null) return false;
            _context.Classes.Remove(@class);
            return _context.SaveChanges() > 0;
        }
    }
}

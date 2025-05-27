using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Exceptions;
using StudentScheduleBackend.Interfaces;

namespace StudentScheduleBackend.Repositories
{
    public class SubjectRepository : IRepository<Subject>
    {
        readonly Context _context;
        public SubjectRepository(Context context) => _context = context;

        public bool Add(Subject subject)
        {
            _context.Subjects.Add(subject);
            return _context.SaveChanges() > 0;
        }

        public List<Subject> GetAll() => _context.Subjects.Include(s => s.Classes).ToList();

        public Subject GetById(int id)
        {
            if(!_context.Subjects.Any(e=>e.Id == id))
                throw new KeyNotFoundException($"Subject with id:{id} could not be found.");

            return _context.Subjects.Include(s => s.Classes).First(s => s.Id == id);
        }

        public bool Update(Subject subject)
        {
            _context.Subjects.Update(subject);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var subject = _context.Subjects.Find(id);
            if (subject == null)
                throw new KeyNotFoundException($"Subject with id:{id} could not be found.");

            if (_context.Classes.Any(c => c.SubjectId == subject.Id))
                throw new ReferentialIntegrityException($"Cannot delete subject with {id} becouse it is assigned to one or more classes.");

            _context.Subjects.Remove(subject);
            return _context.SaveChanges() > 0;
        }
    }
}

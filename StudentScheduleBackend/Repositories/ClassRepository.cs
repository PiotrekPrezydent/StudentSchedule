using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Exceptions;
using StudentScheduleBackend.Interfaces;

namespace StudentScheduleBackend.Repositories
{
    public class ClassRepository : IRepository<Class>
    {
        readonly Context _context;
        public ClassRepository(Context context) => _context = context;

        public bool Add(Class @class)
        {
            if (!_context.Programs.Any(e => e.Id == @class.ProgramId))
                throw new ForeignKeyNotFoundException($"Program with ID {@class.ProgramId} does not exist.");

            if (!_context.Classrooms.Any(e => e.Id == @class.ClassroomId))
                throw new ForeignKeyNotFoundException($"Classroom with ID {@class.ClassroomId} does not exist.");

            if (_context.Subjects.Any(e => e.Id == @class.SubjectId))
                throw new ForeignKeyNotFoundException($"Subject with ID {@class.SubjectId} does not exist.");

            _context.Classes.Add(@class);
            return _context.SaveChanges() > 0;
        }

        public List<Class> GetAll() => _context.Classes
            .Include(c => c.Program)
            .Include(c => c.Subject)
            .Include(c => c.Classroom)
            .ToList();

        public Class GetById(int id)
        {
            if(!_context.Classes.Any(e=>e.Id == id))
                throw new KeyNotFoundException($"Class with id:{id} could not be found.");

            return _context.Classes
            .Include(c => c.Program)
            .Include(c => c.Subject)
            .Include(c => c.Classroom)
            .First(c => c.Id == id);
        } 

        public bool Update(Class @class)
        {
            if(!_context.Classes.Any(e=>e.Id == @class.Id))
                throw new KeyNotFoundException($"Class with id:{@class.Id} could not be found.");

            if (!_context.Programs.Any(e => e.Id == @class.ProgramId))
                throw new ForeignKeyNotFoundException($"Program with ID {@class.ProgramId} does not exist.");

            if (!_context.Classrooms.Any(e => e.Id == @class.ClassroomId))
                throw new ForeignKeyNotFoundException($"Classroom with ID {@class.ClassroomId} does not exist.");

            if (_context.Subjects.Any(e => e.Id == @class.SubjectId))
                throw new ForeignKeyNotFoundException($"Subject with ID {@class.SubjectId} does not exist.");

            _context.ChangeTracker.Clear();
            _context.Classes.Update(@class);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var @class = _context.Classes.Find(id);
            if (@class == null)
                throw new KeyNotFoundException($"Class with id:{id} could not be found.");

            _context.Classes.Remove(@class);
            return _context.SaveChanges() > 0;
        }
    }
}

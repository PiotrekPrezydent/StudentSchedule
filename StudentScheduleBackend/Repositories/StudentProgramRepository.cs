using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Exceptions;

namespace StudentScheduleBackend.Repositories
{
    //cannot implement interface here
    public class StudentProgramRepository
    {
        readonly Context _context;
        public StudentProgramRepository(Context context) => _context = context;

        public bool Add(StudentProgram sp)
        {
            if (!_context.Students.Any(e => e.Id == sp.StudentId))
                throw new ForeignKeyNotFoundException($"Student with ID {sp.StudentId} does not exist.");

            if (!_context.Programs.Any(e => e.Id == sp.ProgramId))
                throw new ForeignKeyNotFoundException($"Program with ID {sp.ProgramId} does not exist.");

            _context.StudentPrograms.Add(sp);
            return _context.SaveChanges() > 0;
        }

        public List<StudentProgram> GetAll() => _context.StudentPrograms
            .Include(sp => sp.Student)
            .Include(sp => sp.Program)
            .ToList();

        public StudentProgram GetById(int studentId, int programId)
        {
            if (!_context.Students.Any(e=>e.Id == studentId))
                throw new ForeignKeyNotFoundException($"Student with ID {studentId} does not exist.");

            if(!_context.Programs.Any(e=>e.Id == programId))
                throw new ForeignKeyNotFoundException($"Program with ID {programId} does not exist.");

            var sp = _context.StudentPrograms.Find(studentId, programId);
            if (sp == null)
                throw new KeyNotFoundException($"Connection between student with id: {studentId} and program with id: {programId} could not be found");

            return _context.StudentPrograms
            .Include(sp => sp.Student)
            .Include(sp => sp.Program)
            .First(sp => sp.StudentId == studentId && sp.ProgramId == programId);
        }

        public bool Update(StudentProgram sp)
        {
            if (!_context.Students.Any(e => e.Id == sp.StudentId))
                throw new ForeignKeyNotFoundException($"Student with ID {sp.StudentId} does not exist.");

            if (!_context.Programs.Any(e => e.Id == sp.ProgramId))
                throw new ForeignKeyNotFoundException($"Program with ID {sp.ProgramId} does not exist.");

            _context.ChangeTracker.Clear();
            _context.StudentPrograms.Update(sp);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int studentId, int programId)
        {
            if (!_context.Students.Any(e => e.Id == studentId))
                throw new ForeignKeyNotFoundException($"Student with ID {studentId} does not exist.");

            if (!_context.Programs.Any(e => e.Id == programId))
                throw new ForeignKeyNotFoundException($"Program with ID {programId} does not exist.");

            var sp = _context.StudentPrograms.Find(studentId, programId);
            if (sp == null)
                throw new KeyNotFoundException($"Connection between student with id: {studentId} and program with id: {programId} could not be found");

            _context.StudentPrograms.Remove(sp);
            return _context.SaveChanges() > 0;
        }
    }
}

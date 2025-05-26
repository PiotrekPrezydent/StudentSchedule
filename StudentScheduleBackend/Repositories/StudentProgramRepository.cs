using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Entities;

namespace StudentScheduleBackend.Repositories
{
    public class StudentProgramRepository
    {
        readonly Context _context;
        public StudentProgramRepository(Context context) => _context = context;

        public bool Add(StudentProgram sp)
        {
            _context.StudentPrograms.Add(sp);
            return _context.SaveChanges() > 0;
        }

        public List<StudentProgram> GetAll() => _context.StudentPrograms
            .Include(sp => sp.Student)
            .Include(sp => sp.Program)
            .ToList();

        public StudentProgram? GetById(int studentId, int programId) => _context.StudentPrograms
            .Include(sp => sp.Student)
            .Include(sp => sp.Program)
            .FirstOrDefault(sp => sp.StudentId == studentId && sp.ProgramId == programId);

        public bool Delete(int studentId, int programId)
        {
            var sp = _context.StudentPrograms.Find(studentId, programId);
            if (sp == null) return false;
            _context.StudentPrograms.Remove(sp);
            return _context.SaveChanges() > 0;
        }
    }
}

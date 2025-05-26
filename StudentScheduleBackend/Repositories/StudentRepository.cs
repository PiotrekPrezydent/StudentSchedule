using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Entities;

namespace StudentScheduleBackend.Repositories
{
    public class StudentRepository
    {
        readonly Context _context;
        public StudentRepository(Context context) => _context = context;

        public bool Add(Student student)
        {
            _context.Students.Add(student);
            return _context.SaveChanges() > 0;
        }

        public List<Student> GetAll() => _context.Students.Include(s => s.Account).Include(s => s.StudentPrograms).ToList();

        public Student? GetById(int id) => _context.Students.Include(s => s.Account).Include(s => s.StudentPrograms).FirstOrDefault(s => s.Id == id);

        public bool Update(Student student)
        {
            _context.Students.Update(student);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null) return false;
            _context.Students.Remove(student);
            return _context.SaveChanges() > 0;
        }
    }
}

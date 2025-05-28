using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Exceptions;
using StudentScheduleBackend.Interfaces;

namespace StudentScheduleBackend.Repositories
{
    public class StudentRepository : IRepository<Student>
    {
        readonly Context _context;
        public StudentRepository(Context context) => _context = context;

        public bool Add(Student student)
        {
            if(!_context.Accounts.Any(e=>e.Id == student.AccountId))
                throw new KeyNotFoundException($"Account with id:{student.AccountId} could not be found.");
            _context.Students.Add(student);
            return _context.SaveChanges() > 0;
        }

        public List<Student> GetAll() => _context.Students.Include(s => s.Account).Include(s => s.StudentPrograms).ToList();

        public Student GetById(int id)
        {
            if (!_context.Students.Any(e=>e.Id == id))
                throw new KeyNotFoundException($"Student with id:{id} could not be found.");

            return _context.Students.Include(s => s.Account).Include(s => s.StudentPrograms).First(s => s.Id == id);
        }

        public bool Update(Student student)
        {
            if(!_context.Students.Any(e=>e.Id == student.Id))
                throw new KeyNotFoundException($"Student with id:{student.Id} could not be found.");

            if (!_context.Accounts.Any(e => e.Id == student.AccountId))
                throw new KeyNotFoundException($"Account with id:{student.AccountId} could not be found.");

            _context.ChangeTracker.Clear();
            _context.Students.Update(student);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
                throw new KeyNotFoundException($"Student with id:{id} could not be found.");

            _context.ChangeTracker.Clear();
            _context.Students.Remove(student);
            return _context.SaveChanges() > 0;
        }
    }
}
using System;
using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Exceptions;
using StudentScheduleBackend.Interfaces;

namespace StudentScheduleBackend.Repositories
{
    public class ClassroomRepository : IRepository<Classroom>
    {
        readonly Context _context;
        public ClassroomRepository(Context context) => _context = context;

        public bool Add(Classroom classroom)
        {
            _context.Classrooms.Add(classroom);
            return _context.SaveChanges() > 0;
        }

        public List<Classroom> GetAll() => _context.Classrooms.Include(c => c.Classes).ToList();

        public Classroom GetById(int id)
        {
            if (_context.Classrooms.Any(e=>e.Id == id))
                throw new KeyNotFoundException($"Classroom with id:{id} could not be found.");

            return _context.Classrooms.Include(c => c.Classes).First(c => c.Id == id);
        }

        public bool Update(Classroom classroom)
        {
            _context.Classrooms.Update(classroom);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var classroom = _context.Classrooms.Find(id);
            if (classroom == null)
                throw new KeyNotFoundException($"Classroom with id:{id} could not be found.");

            if (_context.Classes.Any(e => e.ClassroomId == classroom.Id))
                throw new ReferentialIntegrityException($"cannot delete classroom with {id} becouse it is assigned to one or more classes");

            _context.Classrooms.Remove(classroom);
            return _context.SaveChanges() > 0;
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Exceptions;
using StudentScheduleBackend.Interfaces;

namespace StudentScheduleBackend.Repositories
{
    public class ProgramRepository : IRepository<Program>
    {
        readonly Context _context;
        public ProgramRepository(Context context) => _context = context;

        public bool Add(Program program)
        {
            _context.Programs.Add(program);
            return _context.SaveChanges() > 0;
        }

        public List<Program> GetAll() => _context.Programs.Include(p => p.StudentPrograms).Include(p => p.Classes).ToList();

        public Program GetById(int id)
        {
            if(!_context.Programs.Any(e=>e.Id == id))
                throw new KeyNotFoundException($"program with id:{id} could not be found");

            return _context.Programs.Include(p => p.StudentPrograms).Include(p => p.Classes).First(p => p.Id == id);
        }

        public bool Update(Program program)
        {
            if(!_context.Programs.Any(e=>e.Id == program.Id))
                throw new KeyNotFoundException($"Program with id:{program.Id} could not be found.");

            _context.ChangeTracker.Clear();
            _context.Programs.Update(program);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var program = _context.Programs.Find(id);
            if (program == null)
                throw new KeyNotFoundException($"program with id:{id} could not be found");

            StudentProgramRepository spr = new(_context);

            if(_context.Classes.Any(e=>e.ProgramId == program.Id))
                throw new ReferentialIntegrityException($"cannot delete program with {id} becouse it is assigned to one or more classes");

            if(_context.StudentPrograms.Any(e=>e.ProgramId == program.Id))
               throw new ReferentialIntegrityException($"cannot delete program with {id} becouse it is assigned to one or more students");

            _context.Programs.Remove(program);
            return _context.SaveChanges() > 0;
        }
    }
}

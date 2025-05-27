using System;
using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Entities;

namespace StudentScheduleBackend.Repositories
{
    public class ProgramRepository
    {
        readonly Context _context;
        public ProgramRepository(Context context) => _context = context;

        public bool Add(Program program)
        {
            _context.Programs.Add(program);
            return _context.SaveChanges() > 0;
        }

        public List<Program> GetAll() => _context.Programs.Include(p => p.StudentPrograms).Include(p => p.Classes).ToList();

        public Program? GetById(int id) => _context.Programs.Include(p => p.StudentPrograms).Include(p => p.Classes).FirstOrDefault(p => p.Id == id);

        public bool Update(Program program)
        {
            _context.Programs.Update(program);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var program = _context.Programs.Find(id);
            if (program == null) return false;
            ClassRepository cr = new(_context);
            StudentProgramRepository spr = new(_context);

            if(cr.GetAll().Any(e=>e.ProgramId == program.Id) ||  spr.GetAll().Any(e=>e.ProgramId == program.Id))
                return false;

            _context.Programs.Remove(program);
            return _context.SaveChanges() > 0;
        }
    }
}

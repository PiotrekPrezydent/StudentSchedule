using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Entities;
using StudentScheduleBackend.Exceptions;
using StudentScheduleBackend.Interfaces;

namespace StudentScheduleBackend.Repositories
{
    public class AccountRepository : IRepository<Account>
    {
        readonly Context _context;
        public AccountRepository(Context context) => _context = context;

        public bool Add(Account account)
        {
            if (!_context.Students.Any(e => e.Id == account.StudentId))
                throw new ForeignKeyNotFoundException($"Student with ID {account.StudentId} does not exist.");

            _context.Accounts.Add(account);
            return _context.SaveChanges() > 0;
        }

        public List<Account> GetAll() => _context.Accounts.Include(a => a.Student).ToList();

        public Account GetById(int id)
        {
            if(!_context.Accounts.Any(e=>e.Id == id))
                throw new KeyNotFoundException($"Account with id:{id} could not be found.");

            return _context.Accounts.Include(a => a.Student).First(a => a.Id == id);
        }

        public bool Update(Account account)
        {
            if(!_context.Accounts.Any(e=>e.Id == account.Id))
                throw new KeyNotFoundException($"Account with id:{account.Id} could not be found.");

            if (!_context.Students.Any(e => e.Id == account.StudentId))
                throw new ForeignKeyNotFoundException($"Student with ID {account.StudentId} does not exist.");

            _context.Accounts.Update(account);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null)
                throw new KeyNotFoundException($"Account with id:{id} could not be found.");

            _context.ChangeTracker.Clear();
            _context.Accounts.Remove(account);
            return _context.SaveChanges() > 0;
        }
    }
}

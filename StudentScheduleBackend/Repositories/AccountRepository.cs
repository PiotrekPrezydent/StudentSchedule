using Microsoft.EntityFrameworkCore;
using StudentScheduleBackend.Entities;

namespace StudentScheduleBackend.Repositories
{
    public class AccountRepository
    {
        readonly Context _context;
        public AccountRepository(Context context) => _context = context;

        public bool Add(Account account)
        {
            _context.Accounts.Add(account);
            return _context.SaveChanges() > 0;
        }

        public List<Account> GetAll() => _context.Accounts.Include(a => a.Student).ToList();

        public Account? GetById(int id) => _context.Accounts.Include(a => a.Student).FirstOrDefault(a => a.Id == id);

        public bool Update(Account account)
        {
            _context.Accounts.Update(account);
            return _context.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null) return false;
            _context.Accounts.Remove(account);
            return _context.SaveChanges() > 0;
        }
    }
}

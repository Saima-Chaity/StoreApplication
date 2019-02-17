using StoreApplication.Data;
using StoreApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplication.Repositories
{
    public class UserRepo
    {
        ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context)
        {
            this._context = context;
        }

        // Get all users in the databaFse.
        public IEnumerable<UserVM> All()
        {
            var users = _context.Users.Select(u => new UserVM()
            {
                Email = u.Email
            });
            return users;
        }

        public bool DeleteUser(string email)
        {
            var user = _context.Users.Where(u => u.Email == email).FirstOrDefault();

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }

    }

}

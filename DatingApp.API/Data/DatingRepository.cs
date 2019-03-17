using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        public DataContext _context { get; }

        public DatingRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            return await
                _context.Users
                    .Include(x => x.Photos)
                    .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            return await 
                _context.Photos
                    .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await
                _context.Users
                    .Include(x => x.Photos)
                    .ToListAsync();
        }
    }
}
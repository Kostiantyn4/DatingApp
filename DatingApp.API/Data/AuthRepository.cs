using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Helpers;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Check is user already exist in the system
        /// </summary>
        /// <param name="username">name of the user</param>
        /// <returns>true - if user exist, false - if there is no such user</returns>
        public async Task<bool> IsUserExist(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
            }

            return false;
        }

        /// <summary>
        /// Log in
        /// </summary>
        /// <param name="username">name of the user</param>
        /// <param name="password">password of the user</param>
        /// <returns>user dto</returns>
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            //auth success
            return user;
        }

        /// <summary>
        /// Register new user 
        /// </summary>
        /// <param name="user">user dto</param>
        /// <param name="password">password</param>
        /// <returns>user dto of registerd user</returns>
        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            AuthHelper.Current.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            }
            return true;
        }

    }
}
using System.Collections.Generic;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public readonly DataContext _context;
        public Seed(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get test data from the file and populate it to the database;
        /// </summary>
        public void SeedTestUsers()
        {
            var userData = System.IO.File.ReadAllText("Helpers/TestUserData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt;

                AuthHelper.Current.CreatePasswordHash("password", out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.AddIfNotExists(user);
            }

            _context.SaveChanges();
        }
    }
}
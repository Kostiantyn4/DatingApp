using System;
using System.Linq;
using System.Linq.Expressions;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Add application error to the Header of the Response
        /// </summary>
        /// <param name="responce"></param>
        /// <param name="message">Message that need to add</param>
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        /// <summary>
        /// Insert value to the table if it not already exists
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        public static EntityEntry<T> AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) where T : class, new()
        {
            var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
            return !exists ? dbSet.Add(entity) : null;
        }

        /// <summary>
        /// Calculate the age by date of birth
        /// </summary>
        /// <param name="dateOfBirth">Date of birth</param>
        /// <returns>Age</returns>
        public static int GetAge(this DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;

            if (dateOfBirth.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }
    }
}
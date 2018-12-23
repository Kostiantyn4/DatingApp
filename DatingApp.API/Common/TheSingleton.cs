using System;

namespace DatingApp.API.Common
{
    public abstract class TheSingleton<T> where T : class
    {
        public static T Current { get; }

        static TheSingleton()
        {
            try
            {
                Current = (T)Activator.CreateInstance(typeof(T), true);
            }
            catch (Exception)
            { }
        }
    }
}
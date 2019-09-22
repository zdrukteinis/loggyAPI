using System;

namespace loggyAPI.Services
{
    public class AppException : Exception
    {
        public AppException(string message) : base(message)
        {
        }
    }
}

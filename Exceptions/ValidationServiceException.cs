using System;

namespace Common.Exceptions
{
    public class ValidationServiceException : Exception
    {
        public ValidationServiceException()
        {
        }

        public ValidationServiceException(string message)
            : base(message)
        {
        }

        public ValidationServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

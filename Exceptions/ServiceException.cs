namespace Sadas_test.Exceptions
{
    public class ServiceException
    {
        public class InvalidSourceConfigurationException : Exception
        {
            public InvalidSourceConfigurationException(string message) : base(message) { }
        }
    }
}

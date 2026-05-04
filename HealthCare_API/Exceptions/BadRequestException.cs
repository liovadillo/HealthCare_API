namespace HealthCare_API.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message, 400)
        {
        }
    }
}

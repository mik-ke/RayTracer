namespace RayTracer.Common.Library.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string message, params object[] args)
        : base(string.Format(message, args))
    {
    }
}

using RayTracer.Common.Library.Exceptions;
using RayTracer.Common.Library.Interfaces;

namespace RayTracer.Common.Library.Validation;

public abstract class ValidationBase<T> : IValidation where T : class
{
    protected T Context { get; private set; }

    protected ValidationBase(T context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));
        Context = context;
    }

    public void Validate()
    {
        if (!isValid) throw new ValidationException(Message);
    }

    public abstract bool isValid { get; }
    public abstract string Message { get; }
}

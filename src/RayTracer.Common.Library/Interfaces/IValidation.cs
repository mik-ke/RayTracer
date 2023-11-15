namespace RayTracer.Common.Library.Interfaces;

/// <summary>
/// Basic validation interface.
/// Inspired by Alex Siepman's blog post https://www.siepman.nl/blog/a-solid-validation-class.
/// </summary>
public interface IValidation
{
    /// <summary>
    /// True when valid
    /// </summary>
    bool isValid { get; }

    /// <summary>
    /// Throws an exception when not valid
    /// </summary>
    void Validate();

    /// <summary>
    /// The message when the object is not valid
    /// </summary>
    string Message { get; }
}

namespace ProjectDataManager.Contracts.Exceptions;

public class InsufficientComponentsException : Exception
{
    public InsufficientComponentsException(string message) : base(message) { }

    public InsufficientComponentsException(string message, Exception exception) : base(message, exception) { }
}

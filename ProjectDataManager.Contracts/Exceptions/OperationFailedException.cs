namespace ProjectDataManager.Contracts.Exceptions;

public class OperationFailedException : Exception
{
    public OperationFailedException(string message) : base(message) { }

    public OperationFailedException(string message, Exception exception) : base(message, exception) { }
}

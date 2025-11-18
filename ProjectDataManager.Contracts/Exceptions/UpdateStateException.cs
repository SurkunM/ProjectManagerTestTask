namespace ProjectDataManager.Contracts.Exceptions;

public class UpdateStateException : Exception
{
    public UpdateStateException(string message) : base(message) { }

    public UpdateStateException(string message, Exception exception) : base(message, exception) { }
}

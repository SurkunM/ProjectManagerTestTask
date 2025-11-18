namespace ProjectDataManager.Contracts.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException(string message) : base(message) { }

    public InvalidCredentialsException(string message, Exception exception) : base(message, exception) { }
}

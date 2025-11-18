namespace ProjectDataManager.Contracts.Exceptions;

public class RegistrationFailedException : Exception
{
    public RegistrationFailedException(string message) : base(message) { }

    public RegistrationFailedException(string message, Exception exception) : base(message, exception) { }
}

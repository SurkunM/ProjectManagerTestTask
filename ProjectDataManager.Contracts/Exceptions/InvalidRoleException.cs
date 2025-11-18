namespace ProjectDataManager.Contracts.Exceptions;

public class InvalidRoleException : Exception
{
    public InvalidRoleException(string message) : base(message) { }

    public InvalidRoleException(string message, Exception exception) : base(message, exception) { }
}

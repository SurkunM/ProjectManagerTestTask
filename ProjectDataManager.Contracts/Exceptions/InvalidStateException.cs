namespace ProductionChain.Contracts.Exceptions;

public class InvalidStateException : Exception
{
    public InvalidStateException(string message) : base(message) { }

    public InvalidStateException(string message, Exception exception) : base(message, exception) { }
}

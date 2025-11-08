namespace ProductionChain.Contracts.Exceptions;

public class AccountNotCreatedException : Exception
{
    public AccountNotCreatedException(string message) : base(message) { }

    public AccountNotCreatedException(string message, Exception exception) : base(message, exception) { }
}

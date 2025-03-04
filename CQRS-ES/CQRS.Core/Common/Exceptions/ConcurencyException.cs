namespace CQRS.Core.Common.Exceptions;

public class ConcurencyException : Exception
{
    public ConcurencyException() : base()
    {
    }

    public ConcurencyException(string message) : base(message)
    {
    }
}
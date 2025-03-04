namespace CQRS.Core.Common.Exceptions;

public class AggregateNotFoundException : Exception
{
    public AggregateNotFoundException() : base()
    {
    }

    public AggregateNotFoundException(string message) : base(message)
    {
    }
}
using CQRS.Core.Messages;

namespace CQRS.Core.Commands;

/// <summary>
/// This class represents for the very basic Command.
///	A Command is a combination of expressed intent.
/// In other words, it describes an action that you want to be performed.
/// It also contains the information that is required to undertake the desired action.
/// </summary>
public abstract class BaseCommand : Message
{
}
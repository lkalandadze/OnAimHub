﻿using MediatR;

namespace Shared.Application.Commands;

public abstract class BaseCommand : IRequest<CommandResult> { }

public class CommandResult
{
    private CommandResult(string message = "", string entityId = "")
    {
        EntityId = entityId;
        Message = message;
    }

    public static CommandResult Create(string message = "", string entityId = "")
    {
        return new CommandResult(message, entityId);
    }

    public string EntityId { get; }
    public string Message { get; }
}

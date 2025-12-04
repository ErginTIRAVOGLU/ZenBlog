using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Messages;

public sealed record MessageDeleteCommand(
    Guid MessageId
) : ICommand<Result<bool>>;

public sealed class MessageDeleteCommandValidator : AbstractValidator<MessageDeleteCommand>
{
    public MessageDeleteCommandValidator()
    {
        RuleFor(m => m.MessageId)
            .NotEmpty().WithMessage("Message ID is required.");
    }
}

internal sealed class MessageDeleteCommandHandler(
    IRepository<Message> messageRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<MessageDeleteCommand, Result<bool>>
{
    public async Task<Result<bool>> HandleAsync(MessageDeleteCommand command, CancellationToken cancellationToken)
    {
        var message = await messageRepository.GetByIdAsync(command.MessageId, cancellationToken);
        if (message is null)
        {
            return Result<bool>.Failure(new Error("Message", "Message not found."));
        }

        try
        {
            messageRepository.Delete(message);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Error("Message", ex.Message));
        }

        return Result<bool>.Success(true);
    }
}

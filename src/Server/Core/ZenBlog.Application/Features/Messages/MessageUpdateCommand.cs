using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Messages;

public sealed record MessageUpdateRequest(
    bool IsRead
);

public sealed record MessageUpdateCommand (
    Guid MessageId,
    bool IsRead
) : ICommand<Result<MessageUpdateCommand>>;


public sealed class MessageUpdateCommandValidator : AbstractValidator<MessageUpdateCommand>
{
    public MessageUpdateCommandValidator()
    {
        RuleFor(m => m.MessageId)
            .NotEmpty().WithMessage("Message ID is required.");
    }
}
internal sealed class MessageUpdateCommandHandler(
    IRepository<Message> messageRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<MessageUpdateCommand, Result<MessageUpdateCommand>>
{
    public async Task<Result<MessageUpdateCommand>> HandleAsync(MessageUpdateCommand command, CancellationToken cancellationToken)
    {
        var message = await messageRepository.GetByIdAsync(command.MessageId, cancellationToken);
        if (message is null)
        {
            return Result<MessageUpdateCommand>.Failure(new Error("Message", "Message not found."));
        }

        message.IsRead = command.IsRead;

        try
        {
            messageRepository.Update(message);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<MessageUpdateCommand>.Failure(new Error("Message", ex.Message));
        }

        return Result<MessageUpdateCommand>.Success(command);
    }
}

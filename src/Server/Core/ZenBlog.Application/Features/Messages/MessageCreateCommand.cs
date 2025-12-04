using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Messages;

public sealed record MessageCreateCommand(
    string Name,
    string EMail,
    string Subject,
    string MessageBody
) : ICommand<Result<Guid>>;

public sealed class MessageCreateCommandValidator : AbstractValidator<MessageCreateCommand>
{
    public MessageCreateCommandValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(256).WithMessage("Name must not exceed 256 characters.");

        RuleFor(m => m.EMail)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

        RuleFor(m => m.Subject)
            .NotEmpty().WithMessage("Subject is required.")
            .MaximumLength(256).WithMessage("Subject must not exceed 256 characters.");

        RuleFor(m => m.MessageBody)
            .NotEmpty().WithMessage("Message body is required.")
            .MaximumLength(5000).WithMessage("Message body must not exceed 5000 characters.");
    }
}

internal sealed class MessageCreateCommandHandler(
    IRepository<Message> messageRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<MessageCreateCommand, Result<Guid>>
{
    public async Task<Result<Guid>> HandleAsync(MessageCreateCommand command, CancellationToken cancellationToken)
    {
        var message = new Message
        {
            Name = command.Name,
            EMail = command.EMail,
            Subject = command.Subject,
            MessageBody = command.MessageBody,
            IsRead = false
        };

        try
        {
            await messageRepository.CreateAsync(message, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(new Error("Message", ex.Message));
        }

        return Result<Guid>.Success(message.Id);
    }
}

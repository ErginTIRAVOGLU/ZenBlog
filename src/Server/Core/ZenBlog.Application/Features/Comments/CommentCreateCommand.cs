using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Comments;

public sealed record CommentCreateCommand(
    string Body,
    string UserId,
    Guid BlogId
) : ICommand<Result<Guid>>;

public sealed class CommentCreateCommandValidator : AbstractValidator<CommentCreateCommand>
{
    public CommentCreateCommandValidator()
    {
        RuleFor(c => c.Body)
            .NotEmpty().WithMessage("Comment body is required.")
            .MaximumLength(1000).WithMessage("Comment body must not exceed 1000 characters.");

        RuleFor(c => c.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(c => c.BlogId)
            .NotEmpty().WithMessage("Blog ID is required.");
    }
}

internal sealed class CommentCreateCommandHandler(
    IRepository<Comment> commentRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CommentCreateCommand, Result<Guid>>
{
    public async Task<Result<Guid>> HandleAsync(CommentCreateCommand command, CancellationToken cancellationToken)
    {
        var comment = new Comment
        {
            Body = command.Body,
            CommentDate = DateTime.UtcNow,
            UserId = command.UserId,
            BlogId = command.BlogId
        };

        try
        {
            await commentRepository.CreateAsync(comment, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(new Error("Comment", ex.Message));
        }

        return Result<Guid>.Success(comment.Id);
    }
}

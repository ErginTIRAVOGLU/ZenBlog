using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Comments;

public sealed record CommentUpdateRequest(
    string Body
);

public sealed record CommentUpdateCommand (
    Guid CommentId,
    string Body
) : ICommand<Result<CommentUpdateCommand>>;


public sealed class CommentUpdateCommandValidator : AbstractValidator<CommentUpdateCommand>
{
    public CommentUpdateCommandValidator()
    {
        RuleFor(c => c.CommentId)
            .NotEmpty().WithMessage("Comment ID is required.");

        RuleFor(c => c.Body)
            .NotEmpty().WithMessage("Comment body is required.")
            .MaximumLength(1000).WithMessage("Comment body must not exceed 1000 characters.");
    }
}
internal sealed class CommentUpdateCommandHandler(
    IRepository<Comment> commentRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CommentUpdateCommand, Result<CommentUpdateCommand>>
{
    public async Task<Result<CommentUpdateCommand>> HandleAsync(CommentUpdateCommand command, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(command.CommentId, cancellationToken);
        if (comment is null)
        {
            return Result<CommentUpdateCommand>.Failure(new Error("Comment", "Comment not found."));
        }

        comment.Body = command.Body;

        try
        {
            commentRepository.Update(comment);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<CommentUpdateCommand>.Failure(new Error("Comment", ex.Message));
        }

        return Result<CommentUpdateCommand>.Success(command);
    }
}

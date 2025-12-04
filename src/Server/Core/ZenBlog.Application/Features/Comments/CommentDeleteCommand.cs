using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Comments;

public sealed record CommentDeleteCommand(
    Guid CommentId
) : ICommand<Result<bool>>;

public sealed class CommentDeleteCommandValidator : AbstractValidator<CommentDeleteCommand>
{
    public CommentDeleteCommandValidator()
    {
        RuleFor(c => c.CommentId)
            .NotEmpty().WithMessage("Comment ID is required.");
    }
}

internal sealed class CommentDeleteCommandHandler(
    IRepository<Comment> commentRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CommentDeleteCommand, Result<bool>>
{
    public async Task<Result<bool>> HandleAsync(CommentDeleteCommand command, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(command.CommentId, cancellationToken);
        if (comment is null)
        {
            return Result<bool>.Failure(new Error("Comment", "Comment not found."));
        }

        try
        {
            commentRepository.Delete(comment);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Error("Comment", ex.Message));
        }

        return Result<bool>.Success(true);
    }
}

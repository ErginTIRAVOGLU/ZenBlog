using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.SubComments;

public sealed record SubCommentCreateCommand(
    string Body,
    string UserId,
    Guid CommentId
) : ICommand<Result<Guid>>;

public sealed class SubCommentCreateCommandValidator : AbstractValidator<SubCommentCreateCommand>
{
    public SubCommentCreateCommandValidator()
    {
        RuleFor(sc => sc.Body)
            .NotEmpty().WithMessage("SubComment body is required.")
            .MaximumLength(1000).WithMessage("SubComment body must not exceed 1000 characters.");

        RuleFor(sc => sc.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(sc => sc.CommentId)
            .NotEmpty().WithMessage("Comment ID is required.");
    }
}

internal sealed class SubCommentCreateCommandHandler(
    IRepository<SubComment> subCommentRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<SubCommentCreateCommand, Result<Guid>>
{
    public async Task<Result<Guid>> HandleAsync(SubCommentCreateCommand command, CancellationToken cancellationToken)
    {
        var subComment = new SubComment
        {
            Body = command.Body,
            CommentDate = DateTime.UtcNow,
            UserId = command.UserId,
            CommentId = command.CommentId
        };

        try
        {
            await subCommentRepository.CreateAsync(subComment, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(new Error("SubComment", ex.Message));
        }

        return Result<Guid>.Success(subComment.Id);
    }
}

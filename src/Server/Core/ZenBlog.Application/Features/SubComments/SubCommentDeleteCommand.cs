using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.SubComments;

public sealed record SubCommentDeleteCommand(
    Guid SubCommentId
) : ICommand<Result<bool>>;

public sealed class SubCommentDeleteCommandValidator : AbstractValidator<SubCommentDeleteCommand>
{
    public SubCommentDeleteCommandValidator()
    {
        RuleFor(sc => sc.SubCommentId)
            .NotEmpty().WithMessage("SubComment ID is required.");
    }
}

internal sealed class SubCommentDeleteCommandHandler(
    IRepository<SubComment> subCommentRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<SubCommentDeleteCommand, Result<bool>>
{
    public async Task<Result<bool>> HandleAsync(SubCommentDeleteCommand command, CancellationToken cancellationToken)
    {
        var subComment = await subCommentRepository.GetByIdAsync(command.SubCommentId, cancellationToken);
        if (subComment is null)
        {
            return Result<bool>.Failure(new Error("SubComment", "SubComment not found."));
        }

        try
        {
            subCommentRepository.Delete(subComment);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Error("SubComment", ex.Message));
        }

        return Result<bool>.Success(true);
    }
}

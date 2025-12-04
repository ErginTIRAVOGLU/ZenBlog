using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.SubComments;

public sealed record SubCommentUpdateRequest(
    string Body
);

public sealed record SubCommentUpdateCommand (
    Guid SubCommentId,
    string Body
) : ICommand<Result<SubCommentUpdateCommand>>;


public sealed class SubCommentUpdateCommandValidator : AbstractValidator<SubCommentUpdateCommand>
{
    public SubCommentUpdateCommandValidator()
    {
        RuleFor(sc => sc.SubCommentId)
            .NotEmpty().WithMessage("SubComment ID is required.");

        RuleFor(sc => sc.Body)
            .NotEmpty().WithMessage("SubComment body is required.")
            .MaximumLength(1000).WithMessage("SubComment body must not exceed 1000 characters.");
    }
}
internal sealed class SubCommentUpdateCommandHandler(
    IRepository<SubComment> subCommentRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<SubCommentUpdateCommand, Result<SubCommentUpdateCommand>>
{
    public async Task<Result<SubCommentUpdateCommand>> HandleAsync(SubCommentUpdateCommand command, CancellationToken cancellationToken)
    {
        var subComment = await subCommentRepository.GetByIdAsync(command.SubCommentId, cancellationToken);
        if (subComment is null)
        {
            return Result<SubCommentUpdateCommand>.Failure(new Error("SubComment", "SubComment not found."));
        }

        subComment.Body = command.Body;

        try
        {
            subCommentRepository.Update(subComment);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<SubCommentUpdateCommand>.Failure(new Error("SubComment", ex.Message));
        }

        return Result<SubCommentUpdateCommand>.Success(command);
    }
}

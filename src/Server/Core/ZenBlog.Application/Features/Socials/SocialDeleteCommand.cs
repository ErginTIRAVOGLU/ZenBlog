using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Socials;

public sealed record SocialDeleteCommand(
    Guid SocialId
) : ICommand<Result<bool>>;

public sealed class SocialDeleteCommandValidator : AbstractValidator<SocialDeleteCommand>
{
    public SocialDeleteCommandValidator()
    {
        RuleFor(s => s.SocialId)
            .NotEmpty().WithMessage("Social ID is required.");
    }
}

internal sealed class SocialDeleteCommandHandler(
    IRepository<Social> socialRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<SocialDeleteCommand, Result<bool>>
{
    public async Task<Result<bool>> HandleAsync(SocialDeleteCommand command, CancellationToken cancellationToken)
    {
        var social = await socialRepository.GetByIdAsync(command.SocialId, cancellationToken);
        if (social is null)
        {
            return Result<bool>.Failure(new Error("Social", "Social not found."));
        }

        try
        {
            socialRepository.Delete(social);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Error("Social", ex.Message));
        }

        return Result<bool>.Success(true);
    }
}

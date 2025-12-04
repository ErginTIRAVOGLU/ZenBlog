using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Socials;

public sealed record SocialUpdateRequest(
    string Title,
    string Url,
    string Icon
);

public sealed record SocialUpdateCommand (
    Guid SocialId,
    string Title,
    string Url,
    string Icon
) : ICommand<Result<SocialUpdateCommand>>;


public sealed class SocialUpdateCommandValidator : AbstractValidator<SocialUpdateCommand>
{
    public SocialUpdateCommandValidator()
    {
        RuleFor(s => s.SocialId)
            .NotEmpty().WithMessage("Social ID is required.");

        RuleFor(s => s.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(256).WithMessage("Title must not exceed 256 characters.");

        RuleFor(s => s.Url)
            .NotEmpty().WithMessage("URL is required.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("URL must be a valid URL.")
            .MaximumLength(1000).WithMessage("URL must not exceed 1000 characters.");

        RuleFor(s => s.Icon)
            .NotEmpty().WithMessage("Icon is required.")
            .MaximumLength(256).WithMessage("Icon must not exceed 256 characters.");
    }
}
internal sealed class SocialUpdateCommandHandler(
    IRepository<Social> socialRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<SocialUpdateCommand, Result<SocialUpdateCommand>>
{
    public async Task<Result<SocialUpdateCommand>> HandleAsync(SocialUpdateCommand command, CancellationToken cancellationToken)
    {
        var social = await socialRepository.GetByIdAsync(command.SocialId, cancellationToken);
        if (social is null)
        {
            return Result<SocialUpdateCommand>.Failure(new Error("Social", "Social not found."));
        }

        social.Title = command.Title;
        social.Url = command.Url;
        social.Icon = command.Icon;

        try
        {
            socialRepository.Update(social);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<SocialUpdateCommand>.Failure(new Error("Social", ex.Message));
        }

        return Result<SocialUpdateCommand>.Success(command);
    }
}

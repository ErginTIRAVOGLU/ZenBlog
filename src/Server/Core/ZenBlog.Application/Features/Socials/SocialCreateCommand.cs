using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Socials;

public sealed record SocialCreateCommand(
    string Title,
    string Url,
    string Icon
) : ICommand<Result<Guid>>;

public sealed class SocialCreateCommandValidator : AbstractValidator<SocialCreateCommand>
{
    public SocialCreateCommandValidator()
    {
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

internal sealed class SocialCreateCommandHandler(
    IRepository<Social> socialRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<SocialCreateCommand, Result<Guid>>
{
    public async Task<Result<Guid>> HandleAsync(SocialCreateCommand command, CancellationToken cancellationToken)
    {
        var social = new Social
        {
            Title = command.Title,
            Url = command.Url,
            Icon = command.Icon
        };

        try
        {
            await socialRepository.CreateAsync(social, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(new Error("Social", ex.Message));
        }

        return Result<Guid>.Success(social.Id);
    }
}

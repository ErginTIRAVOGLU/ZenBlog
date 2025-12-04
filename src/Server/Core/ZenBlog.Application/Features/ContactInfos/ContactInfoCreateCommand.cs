using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.ContactInfos;

public sealed record ContactInfoCreateCommand(
    string Address,
    string EMail,
    string Phone,
    string MapUrl
) : ICommand<Result<Guid>>;

public sealed class ContactInfoCreateCommandValidator : AbstractValidator<ContactInfoCreateCommand>
{
    public ContactInfoCreateCommandValidator()
    {
        RuleFor(ci => ci.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(500).WithMessage("Address must not exceed 500 characters.");

        RuleFor(ci => ci.EMail)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .MaximumLength(256).WithMessage("Email must not exceed 256 characters.");

        RuleFor(ci => ci.Phone)
            .NotEmpty().WithMessage("Phone is required.")
            .MaximumLength(20).WithMessage("Phone must not exceed 20 characters.");

        RuleFor(ci => ci.MapUrl)
            .NotEmpty().WithMessage("Map URL is required.")
            .MaximumLength(1000).WithMessage("Map URL must not exceed 1000 characters.");
    }
}

internal sealed class ContactInfoCreateCommandHandler(
    IRepository<ContactInfo> contactInfoRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<ContactInfoCreateCommand, Result<Guid>>
{
    public async Task<Result<Guid>> HandleAsync(ContactInfoCreateCommand command, CancellationToken cancellationToken)
    {
        var contactInfo = new ContactInfo
        {
            Address = command.Address,
            EMail = command.EMail,
            Phone = command.Phone,
            MapUrl = command.MapUrl
        };

        try
        {
            await contactInfoRepository.CreateAsync(contactInfo, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(new Error("ContactInfo", ex.Message));
        }

        return Result<Guid>.Success(contactInfo.Id);
    }
}

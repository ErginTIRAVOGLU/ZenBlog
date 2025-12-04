using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.ContactInfos;

public sealed record ContactInfoUpdateRequest(
    string Address,
    string EMail,
    string Phone,
    string MapUrl
);

public sealed record ContactInfoUpdateCommand (
    Guid ContactInfoId,
    string Address,
    string EMail,
    string Phone,
    string MapUrl
) : ICommand<Result<ContactInfoUpdateCommand>>;


public sealed class ContactInfoUpdateCommandValidator : AbstractValidator<ContactInfoUpdateCommand>
{
    public ContactInfoUpdateCommandValidator()
    {
        RuleFor(ci => ci.ContactInfoId)
            .NotEmpty().WithMessage("ContactInfo ID is required.");

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
internal sealed class ContactInfoUpdateCommandHandler(
    IRepository<ContactInfo> contactInfoRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<ContactInfoUpdateCommand, Result<ContactInfoUpdateCommand>>
{
    public async Task<Result<ContactInfoUpdateCommand>> HandleAsync(ContactInfoUpdateCommand command, CancellationToken cancellationToken)
    {
        var contactInfo = await contactInfoRepository.GetByIdAsync(command.ContactInfoId, cancellationToken);
        if (contactInfo is null)
        {
            return Result<ContactInfoUpdateCommand>.Failure(new Error("ContactInfo", "ContactInfo not found."));
        }

        contactInfo.Address = command.Address;
        contactInfo.EMail = command.EMail;
        contactInfo.Phone = command.Phone;
        contactInfo.MapUrl = command.MapUrl;

        try
        {
            contactInfoRepository.Update(contactInfo);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<ContactInfoUpdateCommand>.Failure(new Error("ContactInfo", ex.Message));
        }

        return Result<ContactInfoUpdateCommand>.Success(command);
    }
}

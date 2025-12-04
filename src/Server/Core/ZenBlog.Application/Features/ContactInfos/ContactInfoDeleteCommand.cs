using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.ContactInfos;

public sealed record ContactInfoDeleteCommand(
    Guid ContactInfoId
) : ICommand<Result<bool>>;

public sealed class ContactInfoDeleteCommandValidator : AbstractValidator<ContactInfoDeleteCommand>
{
    public ContactInfoDeleteCommandValidator()
    {
        RuleFor(ci => ci.ContactInfoId)
            .NotEmpty().WithMessage("ContactInfo ID is required.");
    }
}

internal sealed class ContactInfoDeleteCommandHandler(
    IRepository<ContactInfo> contactInfoRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<ContactInfoDeleteCommand, Result<bool>>
{
    public async Task<Result<bool>> HandleAsync(ContactInfoDeleteCommand command, CancellationToken cancellationToken)
    {
        var contactInfo = await contactInfoRepository.GetByIdAsync(command.ContactInfoId, cancellationToken);
        if (contactInfo is null)
        {
            return Result<bool>.Failure(new Error("ContactInfo", "ContactInfo not found."));
        }

        try
        {
            contactInfoRepository.Delete(contactInfo);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Error("ContactInfo", ex.Message));
        }

        return Result<bool>.Success(true);
    }
}

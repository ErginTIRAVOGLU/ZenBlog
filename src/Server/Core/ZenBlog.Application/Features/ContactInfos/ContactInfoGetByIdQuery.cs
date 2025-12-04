using FluentValidation;
using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.ContactInfos;

public sealed record ContactInfoGetByIdQuery(Guid Id): IQuery<Result<ContactInfoDto>>;

public sealed class ContactInfoGetByIdQueryValidator : AbstractValidator<ContactInfoGetByIdQuery>
{
    public ContactInfoGetByIdQueryValidator()
    {
        RuleFor(ci => ci.Id)
            .NotEmpty().WithMessage("ContactInfo ID is required.");
    }
}

internal sealed class ContactInfoGetByIdQueryHandler(
    IRepository<ContactInfo> contactInfoRepository,
    IMapper mapper
) : IQueryHandler<ContactInfoGetByIdQuery, Result<ContactInfoDto>>
{
    public async Task<Result<ContactInfoDto>> HandleAsync(ContactInfoGetByIdQuery query, CancellationToken cancellationToken)
    {
        var contactInfo = await contactInfoRepository
            .GetByIdAsync(query.Id, cancellationToken: cancellationToken);
        
        if (contactInfo is null)
        {
            return Result<ContactInfoDto>.Failure(new Error("ContactInfo", "Not found."));
        }

        var contactInfoDto = mapper.Map<ContactInfoDto>(contactInfo);
        return Result<ContactInfoDto>.Success(contactInfoDto);
    }
}

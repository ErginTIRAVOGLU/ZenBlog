using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.ContactInfos;

public sealed record ContactInfoGetAllQuery : IQuery<Result<List<ContactInfoDto>>>;


internal sealed class ContactInfoGetAllQueryHandler(
    IRepository<ContactInfo> contactInfoRepository,
    IMapper mapper
) : IQueryHandler<ContactInfoGetAllQuery, Result<List<ContactInfoDto>>>
{
    public async Task<Result<List<ContactInfoDto>>> HandleAsync(ContactInfoGetAllQuery query, CancellationToken cancellationToken)
    {
        var contactInfos = await contactInfoRepository
            .GetAllAsync(cancellationToken: cancellationToken);

        var contactInfoDtos = mapper.Map<List<ContactInfoDto>>(contactInfos);
        return Result<List<ContactInfoDto>>.Success(contactInfoDtos);
    }
}

using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Socials;

public sealed record SocialGetAllQuery : IQuery<Result<List<SocialDto>>>;


internal sealed class SocialGetAllQueryHandler(
    IRepository<Social> socialRepository,
    IMapper mapper
) : IQueryHandler<SocialGetAllQuery, Result<List<SocialDto>>>
{
    public async Task<Result<List<SocialDto>>> HandleAsync(SocialGetAllQuery query, CancellationToken cancellationToken)
    {
        var socials = await socialRepository
            .GetAllAsync(cancellationToken: cancellationToken);

        var socialDtos = mapper.Map<List<SocialDto>>(socials);
        return Result<List<SocialDto>>.Success(socialDtos);
    }
}

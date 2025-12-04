using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.SubComments;

public sealed record SubCommentGetAllQuery : IQuery<Result<List<SubCommentDto>>>;


internal sealed class SubCommentGetAllQueryHandler(
    IRepository<SubComment> subCommentRepository,
    IMapper mapper
) : IQueryHandler<SubCommentGetAllQuery, Result<List<SubCommentDto>>>
{
    public async Task<Result<List<SubCommentDto>>> HandleAsync(SubCommentGetAllQuery query, CancellationToken cancellationToken)
    {
        var subComments = await subCommentRepository
            .GetAllAsync(cancellationToken: cancellationToken);

        var subCommentDtos = mapper.Map<List<SubCommentDto>>(subComments);
        return Result<List<SubCommentDto>>.Success(subCommentDtos);
    }
}

using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Comments;

public sealed record CommentGetAllQuery : IQuery<Result<List<CommentDto>>>;


internal sealed class CommentGetAllQueryHandler(
    IRepository<Comment> commentRepository,
    IMapper mapper
) : IQueryHandler<CommentGetAllQuery, Result<List<CommentDto>>>
{
    public async Task<Result<List<CommentDto>>> HandleAsync(CommentGetAllQuery query, CancellationToken cancellationToken)
    {
        var comments = await commentRepository
            .GetAllAsync(cancellationToken: cancellationToken);

        var commentDtos = mapper.Map<List<CommentDto>>(comments);
        return Result<List<CommentDto>>.Success(commentDtos);
    }
}

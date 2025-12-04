using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Blogs;

public sealed record BlogGetAllQuery : IQuery<Result<List<BlogDto>>>;


internal sealed class BlogGetAllQueryHandler(
    IRepository<Blog> blogRepository,
    IMapper mapper
) : IQueryHandler<BlogGetAllQuery, Result<List<BlogDto>>>
{
    public async Task<Result<List<BlogDto>>> HandleAsync(BlogGetAllQuery query, CancellationToken cancellationToken)
    {
        var blogs = await blogRepository
            .GetAllAsync(cancellationToken: cancellationToken);

        var blogDtos = mapper.Map<List<BlogDto>>(blogs);
        return Result<List<BlogDto>>.Success(blogDtos);
    }
}
 
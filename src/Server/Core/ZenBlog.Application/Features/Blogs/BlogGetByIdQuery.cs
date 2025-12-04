using FluentValidation;
using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Blogs;

public sealed record BlogGetByIdQuery(Guid Id): IQuery<Result<BlogDto>>;

public sealed class BlogGetByIdQueryValidator : AbstractValidator<BlogGetByIdQuery>
{
    public BlogGetByIdQueryValidator()
    {
        RuleFor(b => b.Id)
            .NotEmpty().WithMessage("Blog ID is required.");
    }
}

internal sealed class BlogGetByIdQueryHandler(
    IRepository<Blog> blogRepository,
    IMapper mapper
) : IQueryHandler<BlogGetByIdQuery, Result<BlogDto>>
{
    public async Task<Result<BlogDto>> HandleAsync(BlogGetByIdQuery query, CancellationToken cancellationToken)
    {
        var blog = await blogRepository
            .GetByIdAsync(query.Id, cancellationToken: cancellationToken);
        
        if (blog is null)
        {
            return Result<BlogDto>.Failure(new Error("Blog", "Not found."));
        }

        var blogDto = mapper.Map<BlogDto>(blog);
        return Result<BlogDto>.Success(blogDto);
    }
}
 
using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Categories;

public sealed record CategoryGetAllQuery : IQuery<Result<List<CategoryDto>>>;

internal sealed class CategoryGetAllQueryHandler(
    IRepository<Category> categoryRepository,
    IMapper mapper
) : IQueryHandler<CategoryGetAllQuery, Result<List<CategoryDto>>>
{
    public async Task<Result<List<CategoryDto>>> HandleAsync(CategoryGetAllQuery query, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository
            .GetAllAsync(cancellationToken: cancellationToken);

        var categoryDtos = mapper.Map<List<CategoryDto>>(categories);
        return Result<List<CategoryDto>>.Success(categoryDtos);
    }
}
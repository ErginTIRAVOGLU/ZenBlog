using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Categories;

public sealed record CategoryGetByIdQuery(Guid Id): IQuery<Result<CategoryDto>>;

internal sealed class CategoryGetByIdQueryHandler(
    IRepository<Category> categoryRepository,
    IMapper mapper
) : IQueryHandler<CategoryGetByIdQuery, Result<CategoryDto>>
{
    public async Task<Result<CategoryDto>> HandleAsync(CategoryGetByIdQuery query, CancellationToken cancellationToken)
    {
        var category = await categoryRepository
            .GetByIdAsync(query.Id, cancellationToken: cancellationToken);
        
        if (category is null)
        {
            return Result<CategoryDto>.Failure(new Error("Category", "Not found."));
        }

        var categoryDto = mapper.Map<CategoryDto>(category);
        return Result<CategoryDto>.Success(categoryDto);
    }
}
 
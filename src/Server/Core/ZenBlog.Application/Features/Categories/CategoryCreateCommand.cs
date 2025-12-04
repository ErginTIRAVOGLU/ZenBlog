using FluentValidation;
using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Categories;

public sealed record CategoryCreateCommand(string CategoryName) : ICommand<Result<CategoryDto>>;

public sealed class CategoryCreateCommandValidator : AbstractValidator<CategoryCreateCommand>
{
    public CategoryCreateCommandValidator()
    {
        RuleFor(c => c.CategoryName)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
    }
}

internal sealed class CategoryCreateCommandHandler(
    IRepository<Category> categoryRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork

) : ICommandHandler<CategoryCreateCommand, Result<CategoryDto>>
{
    public async Task<Result<CategoryDto>> HandleAsync(CategoryCreateCommand command, CancellationToken cancellationToken)
    { 
        var category = new Category { CategoryName = command.CategoryName };
        try
        {            
            await categoryRepository.CreateAsync(category, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<CategoryDto>.Failure(new Error("Category", ex.Message));
        }
        
        var categoryDto = mapper.Map<CategoryDto>(category);

        return Result<CategoryDto>.Success(categoryDto);
    }
}
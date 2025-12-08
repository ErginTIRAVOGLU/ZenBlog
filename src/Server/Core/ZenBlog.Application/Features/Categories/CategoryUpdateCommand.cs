using FluentValidation;
using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Categories;

public sealed record CategoryUpdateRequest(
    string CategoryName
);

public sealed record CategoryUpdateCommand(
    Guid CategoryId,
    string CategoryName
) : ICommand<Result<CategoryDto>>;

public sealed class CategoryUpdateCommandValidator : AbstractValidator<CategoryUpdateCommand>
{
    public CategoryUpdateCommandValidator()
    {
        RuleFor(c => c.CategoryName)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.")
            .MinimumLength(3).WithMessage("Category name must be at least 3 characters long.");
    }
}

internal sealed class CategoryUpdateCommandHandler(
    IRepository<Category> categoryRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork
) : ICommandHandler<CategoryUpdateCommand, Result<CategoryDto>>
{
    public async Task<Result<CategoryDto>> HandleAsync(CategoryUpdateCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);
        if (category is null)
        {
            return Result<CategoryDto>.Failure(new Error("Category", "Category not found."));
        }

        category.CategoryName = command.CategoryName; 

        try
        {
            categoryRepository.Update(category);
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
using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Categories;

public sealed record CategoryDeleteCommand (Guid CategoryId) : ICommand<Result<bool>>;

public sealed class CategoryDeleteCommandValidator : AbstractValidator<CategoryDeleteCommand>
{
    public CategoryDeleteCommandValidator()
    {
        RuleFor(c => c.CategoryId)
            .NotEmpty().WithMessage("Category ID is required.");
    }
}

internal sealed class CategoryDeleteCommandHandler(
    IRepository<Category> categoryRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CategoryDeleteCommand, Result<bool>>
{
    public async Task<Result<bool>> HandleAsync(CategoryDeleteCommand command, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);
        if (category is null)
        {
            return Result<bool>.Failure(new Error("Category", "Category not found."));
        }

        try
        {
            categoryRepository.Delete(category);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Error("Category", ex.Message));
        }

        return Result<bool>.Success(true);
    }
}
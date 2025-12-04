using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Blogs;

public sealed record BlogDeleteCommand(
    Guid BlogId
) : ICommand<Result<bool>>;

public sealed class BlogDeleteCommandValidator : AbstractValidator<BlogDeleteCommand>
{
    public BlogDeleteCommandValidator()
    {
        RuleFor(b => b.BlogId)
            .NotEmpty().WithMessage("Blog ID is required.");
    }
}

internal sealed class BlogDeleteCommandHandler(
    IRepository<Blog> blogRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<BlogDeleteCommand, Result<bool>>
{
    public async Task<Result<bool>> HandleAsync(BlogDeleteCommand command, CancellationToken cancellationToken)
    {
        var blog = await blogRepository.GetByIdAsync(command.BlogId, cancellationToken);
        if (blog is null)
        {
            return Result<bool>.Failure(new Error("Blog", "Blog not found."));
        }

        try
        {
            blogRepository.Delete(blog);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(new Error("Blog", ex.Message));
        }

        return Result<bool>.Success(true);
    }
}

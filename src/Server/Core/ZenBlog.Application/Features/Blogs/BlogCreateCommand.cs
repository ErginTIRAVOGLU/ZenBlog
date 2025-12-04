using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Blogs;

public sealed record BlogCreateCommand(
    string Title,
    string CoverImage,
    string ConteBlogImagent,
    string Description,
    string UserId,
    Guid CategoryId
) : ICommand<Result<Guid>>;

public sealed class BlogCreateCommandValidator : AbstractValidator<BlogCreateCommand>
{
    public BlogCreateCommandValidator()
    {
        RuleFor(b => b.Title)
            .NotEmpty().WithMessage("Blog title is required.")
            .MaximumLength(200).WithMessage("Blog title must not exceed 200 characters.");

        RuleFor(b => b.CoverImage)
            .NotEmpty().WithMessage("Cover image is required.");

        RuleFor(b => b.ConteBlogImagent)
            .NotEmpty().WithMessage("Blog image is required.");

        RuleFor(b => b.Description)
            .NotEmpty().WithMessage("Blog description is required.");

        RuleFor(b => b.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(b => b.CategoryId)
            .NotEmpty().WithMessage("Category ID is required.");
    }
}

internal sealed class BlogCreateCommandHandler(
    IRepository<Blog> blogRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<BlogCreateCommand, Result<Guid>>
{
    public async Task<Result<Guid>> HandleAsync(BlogCreateCommand command, CancellationToken cancellationToken)
    {
        var blog = new Blog
        {
            Title = command.Title,
            CoverImage = command.CoverImage,
            BlogImage = command.ConteBlogImagent,
            Description = command.Description,
            UserId = command.UserId,
            CategoryId = command.CategoryId
        };

        try
        {
            await blogRepository.CreateAsync(blog, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(new Error("Blog", ex.Message));
        }

        return Result<Guid>.Success(blog.Id);
    }
}
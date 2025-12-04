using FluentValidation;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Blogs;

public sealed record BlogUpdateRequest(
    string Title,
    string CoverImage,
    string ConteBlogImagent,
    string Description,
    Guid CategoryId
);

public sealed record BlogUpdateCommand (
    Guid BlogId,
    string Title,
    string CoverImage,
    string ConteBlogImagent,
    string Description,
    Guid CategoryId
) : ICommand<Result<BlogUpdateCommand>>;


public sealed class BlogUpdateCommandValidator : AbstractValidator<BlogUpdateCommand>
{
    public BlogUpdateCommandValidator()
    {
        RuleFor(b => b.BlogId)
            .NotEmpty().WithMessage("Blog ID is required.");

        RuleFor(b => b.Title)
            .NotEmpty().WithMessage("Blog title is required.")
            .MaximumLength(200).WithMessage("Blog title must not exceed 200 characters.");

        RuleFor(b => b.CoverImage)
            .NotEmpty().WithMessage("Cover image is required.");

        RuleFor(b => b.ConteBlogImagent)
            .NotEmpty().WithMessage("Blog image is required.");

        RuleFor(b => b.Description)
            .NotEmpty().WithMessage("Blog description is required.");

        RuleFor(b => b.CategoryId)
            .NotEmpty().WithMessage("Category ID is required.");
    }
}
internal sealed class BlogUpdateCommandHandler(
    IRepository<Blog> blogRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<BlogUpdateCommand, Result<BlogUpdateCommand>>
{
    public async Task<Result<BlogUpdateCommand>> HandleAsync(BlogUpdateCommand command, CancellationToken cancellationToken)
    {
        var blog = await blogRepository.GetByIdAsync(command.BlogId, cancellationToken);
        if (blog is null)
        {
            return Result<BlogUpdateCommand>.Failure(new Error("Blog", "Blog not found."));
        }

        blog.Title = command.Title;
        blog.CoverImage = command.CoverImage;
        blog.BlogImage = command.ConteBlogImagent;
        blog.Description = command.Description;
        blog.CategoryId = command.CategoryId;

        try
        {
            blogRepository.Update(blog);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<BlogUpdateCommand>.Failure(new Error("Blog", ex.Message));
        }

        return Result<BlogUpdateCommand>.Success(command);
    }
}


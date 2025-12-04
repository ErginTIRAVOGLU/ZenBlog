using FluentValidation;
using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Comments;

public sealed record CommentGetByIdQuery(Guid Id): IQuery<Result<CommentDto>>;

public sealed class CommentGetByIdQueryValidator : AbstractValidator<CommentGetByIdQuery>
{
    public CommentGetByIdQueryValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Comment ID is required.");
    }
}

internal sealed class CommentGetByIdQueryHandler(
    IRepository<Comment> commentRepository,
    IMapper mapper
) : IQueryHandler<CommentGetByIdQuery, Result<CommentDto>>
{
    public async Task<Result<CommentDto>> HandleAsync(CommentGetByIdQuery query, CancellationToken cancellationToken)
    {
        var comment = await commentRepository
            .GetByIdAsync(query.Id, cancellationToken: cancellationToken);
        
        if (comment is null)
        {
            return Result<CommentDto>.Failure(new Error("Comment", "Not found."));
        }

        var commentDto = mapper.Map<CommentDto>(comment);
        return Result<CommentDto>.Success(commentDto);
    }
}

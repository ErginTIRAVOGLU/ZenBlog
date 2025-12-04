using FluentValidation;
using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.SubComments;

public sealed record SubCommentGetByIdQuery(Guid Id): IQuery<Result<SubCommentDto>>;

public sealed class SubCommentGetByIdQueryValidator : AbstractValidator<SubCommentGetByIdQuery>
{
    public SubCommentGetByIdQueryValidator()
    {
        RuleFor(sc => sc.Id)
            .NotEmpty().WithMessage("SubComment ID is required.");
    }
}

internal sealed class SubCommentGetByIdQueryHandler(
    IRepository<SubComment> subCommentRepository,
    IMapper mapper
) : IQueryHandler<SubCommentGetByIdQuery, Result<SubCommentDto>>
{
    public async Task<Result<SubCommentDto>> HandleAsync(SubCommentGetByIdQuery query, CancellationToken cancellationToken)
    {
        var subComment = await subCommentRepository
            .GetByIdAsync(query.Id, cancellationToken: cancellationToken);
        
        if (subComment is null)
        {
            return Result<SubCommentDto>.Failure(new Error("SubComment", "Not found."));
        }

        var subCommentDto = mapper.Map<SubCommentDto>(subComment);
        return Result<SubCommentDto>.Success(subCommentDto);
    }
}

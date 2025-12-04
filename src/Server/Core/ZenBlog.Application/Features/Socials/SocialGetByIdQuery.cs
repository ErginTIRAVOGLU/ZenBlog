using FluentValidation;
using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Socials;

public sealed record SocialGetByIdQuery(Guid Id): IQuery<Result<SocialDto>>;

public sealed class SocialGetByIdQueryValidator : AbstractValidator<SocialGetByIdQuery>
{
    public SocialGetByIdQueryValidator()
    {
        RuleFor(s => s.Id)
            .NotEmpty().WithMessage("Social ID is required.");
    }
}

internal sealed class SocialGetByIdQueryHandler(
    IRepository<Social> socialRepository,
    IMapper mapper
) : IQueryHandler<SocialGetByIdQuery, Result<SocialDto>>
{
    public async Task<Result<SocialDto>> HandleAsync(SocialGetByIdQuery query, CancellationToken cancellationToken)
    {
        var social = await socialRepository
            .GetByIdAsync(query.Id, cancellationToken: cancellationToken);
        
        if (social is null)
        {
            return Result<SocialDto>.Failure(new Error("Social", "Not found."));
        }

        var socialDto = mapper.Map<SocialDto>(social);
        return Result<SocialDto>.Success(socialDto);
    }
}

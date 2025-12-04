using FluentValidation;
using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Messages;

public sealed record MessageGetByIdQuery(Guid Id): IQuery<Result<MessageDto>>;

public sealed class MessageGetByIdQueryValidator : AbstractValidator<MessageGetByIdQuery>
{
    public MessageGetByIdQueryValidator()
    {
        RuleFor(m => m.Id)
            .NotEmpty().WithMessage("Message ID is required.");
    }
}

internal sealed class MessageGetByIdQueryHandler(
    IRepository<Message> messageRepository,
    IMapper mapper
) : IQueryHandler<MessageGetByIdQuery, Result<MessageDto>>
{
    public async Task<Result<MessageDto>> HandleAsync(MessageGetByIdQuery query, CancellationToken cancellationToken)
    {
        var message = await messageRepository
            .GetByIdAsync(query.Id, cancellationToken: cancellationToken);
        
        if (message is null)
        {
            return Result<MessageDto>.Failure(new Error("Message", "Not found."));
        }

        var messageDto = mapper.Map<MessageDto>(message);
        return Result<MessageDto>.Success(messageDto);
    }
}

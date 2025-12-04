using Kommand.Abstractions;
using MapsterMapper;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Dto;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Messages;

public sealed record MessageGetAllQuery : IQuery<Result<List<MessageDto>>>;


internal sealed class MessageGetAllQueryHandler(
    IRepository<Message> messageRepository,
    IMapper mapper
) : IQueryHandler<MessageGetAllQuery, Result<List<MessageDto>>>
{
    public async Task<Result<List<MessageDto>>> HandleAsync(MessageGetAllQuery query, CancellationToken cancellationToken)
    {
        var messages = await messageRepository
            .GetAllAsync(cancellationToken: cancellationToken);

        var messageDtos = mapper.Map<List<MessageDto>>(messages);
        return Result<List<MessageDto>>.Success(messageDtos);
    }
}

using Kommand.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Features.Messages;
using ZenBlog.Domain.Dto;

namespace ZenBlog.API.Endpoints;

public static class MessageEndpoints
{
    public static void MapMessageEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/messages")
            .WithTags("Messages");

        group.MapGet("/", GetAllMessages)
          .WithName("GetAllMessages");

        group.MapGet("/{id:guid}", GetMessageById)
            .WithName("GetMessageById");

        group.MapPost("/", CreateMessage)
            .WithName("CreateMessage");

        group.MapPut("/{id:guid}", UpdateMessage)
            .WithName("UpdateMessage");

        group.MapDelete("/{id:guid}", DeleteMessage)
            .WithName("DeleteMessage");

    }

    private static async Task<IResult> GetAllMessages(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new MessageGetAllQuery();
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<IList<MessageDto>>.Success(result.Data!))
            : Results.BadRequest(Result<IList<MessageDto>>.Failure(result.Errors!));
    }

    private static async Task<IResult> GetMessageById(
        [FromServices] IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new MessageGetByIdQuery(id);
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<MessageDto>.Success(result.Data!))
            : Results.NotFound(Result<MessageDto>.Failure(new Error("Message", "Not found.")));
    }

    private static async Task<IResult> CreateMessage(
        [FromServices] IMediator mediator,
        MessageCreateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/messages/{result.Data}", Result<Guid>.Success(result.Data!))
            : Results.BadRequest(Result<Guid>.Failure(result.Errors!));
    }

    private static async Task<IResult> UpdateMessage(
        [FromServices] IMediator mediator,
        Guid id,
        MessageUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var updateCommand = new MessageUpdateCommand(id, request.IsRead);

        var result = await mediator.SendAsync(updateCommand, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<MessageUpdateCommand>.Success(result.Data!))
            : Results.BadRequest(Result<MessageUpdateCommand>.Failure(result.Errors!));
    }

    private static async Task<IResult> DeleteMessage(
        [FromServices] IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new MessageDeleteCommand(id);
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<bool>.Success(true))
            : Results.BadRequest(Result<bool>.Failure(result.Errors!));
    }
}

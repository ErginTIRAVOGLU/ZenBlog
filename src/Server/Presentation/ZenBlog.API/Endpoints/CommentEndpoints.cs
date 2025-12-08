using Kommand.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Features.Comments;
using ZenBlog.Domain.Dto;

namespace ZenBlog.API.Endpoints;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/comments")
            .WithTags("Comments");

        group.MapGet("/", GetAllComments)
          .WithName("GetAllComments");

        group.MapGet("/{id:guid}", GetCommentById)
            .WithName("GetCommentById");

        group.MapPost("/", CreateComment)
            .WithName("CreateComment");

        group.MapPut("/{id:guid}", UpdateComment)
            .WithName("UpdateComment");

        group.MapDelete("/{id:guid}", DeleteComment)
            .WithName("DeleteComment");

    }

    private static async Task<IResult> GetAllComments(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new CommentGetAllQuery();
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<IList<CommentDto>>.Success(result.Data!))
            : Results.BadRequest(Result<IList<CommentDto>>.Failure(result.Errors!));
    }

    private static async Task<IResult> GetCommentById(
        [FromServices] IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new CommentGetByIdQuery(id);
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<CommentDto>.Success(result.Data!))
            : Results.NotFound(Result<CommentDto>.Failure(new Error("Comment", "Not found.")));
    }

    private static async Task<IResult> CreateComment(
        [FromServices] IMediator mediator,
        CommentCreateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/comments/{result.Data}", Result<Guid>.Success(result.Data!))
            : Results.BadRequest(Result<Guid>.Failure(result.Errors!));
    }

    private static async Task<IResult> UpdateComment(
        [FromServices] IMediator mediator,
        Guid id,
        CommentUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var updateCommand = new CommentUpdateCommand(id, request.Body);

        var result = await mediator.SendAsync(updateCommand, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<CommentUpdateCommand>.Success(result.Data!))
            : Results.BadRequest(Result<CommentUpdateCommand>.Failure(result.Errors!));
    }

    private static async Task<IResult> DeleteComment(
        [FromServices] IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new CommentDeleteCommand(id);
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<bool>.Success(true))
            : Results.BadRequest(Result<bool>.Failure(result.Errors!));
    }
}

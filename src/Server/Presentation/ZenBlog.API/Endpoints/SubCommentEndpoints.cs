using Kommand.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Features.SubComments;
using ZenBlog.Domain.Dto;

namespace ZenBlog.API.Endpoints;

public static class SubCommentEndpoints
{
    public static void MapSubCommentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/sub-comments")
            .WithTags("SubComments");

        group.MapGet("/", GetAllSubComments)
          .WithName("GetAllSubComments");

        group.MapGet("/{id:guid}", GetSubCommentById)
            .WithName("GetSubCommentById");

        group.MapPost("/", CreateSubComment)
            .WithName("CreateSubComment");

        group.MapPut("/{id:guid}", UpdateSubComment)
            .WithName("UpdateSubComment");

        group.MapDelete("/{id:guid}", DeleteSubComment)
            .WithName("DeleteSubComment");

    }

    private static async Task<IResult> GetAllSubComments(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new SubCommentGetAllQuery();
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<IList<SubCommentDto>>.Success(result.Data!))
            : Results.BadRequest(Result<IList<SubCommentDto>>.Failure(result.Errors!));
    }

    private static async Task<IResult> GetSubCommentById(
        [FromServices] IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new SubCommentGetByIdQuery(id);
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<SubCommentDto>.Success(result.Data!))
            : Results.NotFound(Result<SubCommentDto>.Failure(new Error("SubComment", "Not found.")));
    }

    private static async Task<IResult> CreateSubComment(
        [FromServices] IMediator mediator,
        SubCommentCreateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/sub-comments/{result.Data}", Result<Guid>.Success(result.Data!))
            : Results.BadRequest(Result<Guid>.Failure(result.Errors!));
    }

    private static async Task<IResult> UpdateSubComment(
        [FromServices] IMediator mediator,
        Guid id,
        SubCommentUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var updateCommand = new SubCommentUpdateCommand(id, request.Body);

        var result = await mediator.SendAsync(updateCommand, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<SubCommentUpdateCommand>.Success(result.Data!))
            : Results.BadRequest(Result<SubCommentUpdateCommand>.Failure(result.Errors!));
    }

    private static async Task<IResult> DeleteSubComment(
        [FromServices] IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new SubCommentDeleteCommand(id);
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<bool>.Success(true))
            : Results.BadRequest(Result<bool>.Failure(result.Errors!));
    }
}

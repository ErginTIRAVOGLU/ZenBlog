using Kommand.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Features.Socials;
using ZenBlog.Domain.Dto;

namespace ZenBlog.API.Endpoints;

public static class SocialsEndpoints
{
    public static void MapSocialEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/socials")
            .WithTags("Socials");

        group.MapGet("/", GetAllSocials)
          .WithName("GetAllSocials");

        group.MapGet("/{id:guid}", GetSocialById)
            .WithName("GetSocialById");

        group.MapPost("/", CreateSocial)
            .WithName("CreateSocial");

        group.MapPut("/{id:guid}", UpdateSocial)
            .WithName("UpdateSocial");

        group.MapDelete("/{id:guid}", DeleteSocial)
            .WithName("DeleteSocial");

    }

    private static async Task<IResult> GetAllSocials(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new SocialGetAllQuery();
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<IList<SocialDto>>.Success(result.Data!))
            : Results.BadRequest(Result<IList<SocialDto>>.Failure(result.Errors!));
    }

    private static async Task<IResult> GetSocialById(
        [FromServices] IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new SocialGetByIdQuery(id);
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<SocialDto>.Success(result.Data!))
            : Results.NotFound(Result<SocialDto>.Failure(new Error("Social", "Not found.")));
    }

    private static async Task<IResult> CreateSocial(
        [FromServices] IMediator mediator,
        SocialCreateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/socials/{result.Data}", Result<Guid>.Success(result.Data!))
            : Results.BadRequest(Result<Guid>.Failure(result.Errors!));
    }

    private static async Task<IResult> UpdateSocial(
        [FromServices] IMediator mediator,
        Guid id,
        SocialUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var updateCommand = new SocialUpdateCommand(id, request.Title, request.Url, request.Icon);

        var result = await mediator.SendAsync(updateCommand, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<SocialUpdateCommand>.Success(result.Data!))
            : Results.BadRequest(Result<SocialUpdateCommand>.Failure(result.Errors!));
    }

    private static async Task<IResult> DeleteSocial(
        [FromServices] IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new SocialDeleteCommand(id);
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<bool>.Success(true))
            : Results.BadRequest(Result<bool>.Failure(result.Errors!));
    }
}

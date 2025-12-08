using Kommand.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Features.ContactInfos;
using ZenBlog.Domain.Dto;

namespace ZenBlog.API.Endpoints;

public static class ContactInfoEndpoints
{
    public static void MapContactInfoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/contact-infos")
            .WithTags("ContactInfos");

        group.MapGet("/", GetAllContactInfos)
          .WithName("GetAllContactInfos");

        group.MapGet("/{id:guid}", GetContactInfoById)
            .WithName("GetContactInfoById");

        group.MapPost("/", CreateContactInfo)
            .WithName("CreateContactInfo");

        group.MapPut("/{id:guid}", UpdateContactInfo)
            .WithName("UpdateContactInfo");

        group.MapDelete("/{id:guid}", DeleteContactInfo)
            .WithName("DeleteContactInfo");

    }

    private static async Task<IResult> GetAllContactInfos(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    {
        var query = new ContactInfoGetAllQuery();
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<IList<ContactInfoDto>>.Success(result.Data!))
            : Results.BadRequest(Result<IList<ContactInfoDto>>.Failure(result.Errors!));
    }

    private static async Task<IResult> GetContactInfoById(
        [FromServices] IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new ContactInfoGetByIdQuery(id);
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<ContactInfoDto>.Success(result.Data!))
            : Results.NotFound(Result<ContactInfoDto>.Failure(new Error("ContactInfo", "Not found.")));
    }

    private static async Task<IResult> CreateContactInfo(
        [FromServices] IMediator mediator,
        ContactInfoCreateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/contact-infos/{result.Data}", Result<Guid>.Success(result.Data!))
            : Results.BadRequest(Result<Guid>.Failure(result.Errors!));
    }

    private static async Task<IResult> UpdateContactInfo(
        [FromServices] IMediator mediator,
        Guid id,
        ContactInfoUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var updateCommand = new ContactInfoUpdateCommand(id, request.Address, request.EMail, request.Phone, request.MapUrl);

        var result = await mediator.SendAsync(updateCommand, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<ContactInfoUpdateCommand>.Success(result.Data!))
            : Results.BadRequest(Result<ContactInfoUpdateCommand>.Failure(result.Errors!));
    }

    private static async Task<IResult> DeleteContactInfo(
        [FromServices] IMediator mediator,
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new ContactInfoDeleteCommand(id);
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<bool>.Success(true))
            : Results.BadRequest(Result<bool>.Failure(result.Errors!));
    }
}

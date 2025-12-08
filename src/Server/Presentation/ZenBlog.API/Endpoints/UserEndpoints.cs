using System;
using Kommand.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Features.Users;

namespace ZenBlog.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users")
            .WithTags("Users");

        group.MapPost("/", CreateUser)
            .WithName("CreateUser");
    }

    private static async Task<IResult> CreateUser(
        [FromServices] IMediator mediator,
        UserCreateCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<UserCreateResult>.Success(result.Data!))
            : Results.BadRequest(Result<UserCreateResult>.Failure(result.Errors!));
    }



}

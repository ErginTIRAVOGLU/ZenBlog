using System;
using Kommand.Abstractions;
using Microsoft.AspNetCore.Mvc;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Features.Users;

namespace ZenBlog.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").AllowAnonymous()
            .WithTags("Authentication");
        group.MapPost("/login", Login)
            .WithName("Login");
       
    }

    private static async Task<IResult> Login(
        [FromServices] IMediator mediator,
        GetLoginQuery query,
        CancellationToken cancellationToken)
    {
        var result = await mediator.QueryAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(Result<GetLoginQueryResult>.Success(result.Data!))
            : Results.BadRequest(Result<GetLoginQueryResult>.Failure(result.Errors!));
    }
}
using System;
using FluentValidation;
using Kommand.Abstractions;
using Microsoft.AspNetCore.Identity;
using ZenBlog.Application.Concrete;
using ZenBlog.Application.Contracts.Persistence;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Users;

public sealed class GetLoginQueryResult
{
    public string Token { get; set; } = default!;
    public DateTime Expiration { get; set; }
}


public sealed record GetLoginQuery(string EmailOrPassword, string Password) : IQuery<Result<GetLoginQueryResult>>;

public sealed class GetLoginValidation : AbstractValidator<GetLoginQuery>
{
    public GetLoginValidation()
    {
        RuleFor(x => x.EmailOrPassword)
            .NotEmpty().WithMessage("Email or Password is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}

internal sealed class GetLoginQueryHandler(
    IJwtService jwtService,
    UserManager<AppUser> userManager
    ) : IQueryHandler<GetLoginQuery, Result<GetLoginQueryResult>>
{
    public async Task<Result<GetLoginQueryResult>> HandleAsync(GetLoginQuery query, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(query.EmailOrPassword);
        if (user is null)
        {
            user= await userManager.FindByNameAsync(query.EmailOrPassword);
            if (user is null)
            {
                return Result<GetLoginQueryResult>.Failure( new Error("Email Or Password", "Invalid email or password."));
            }
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, query.Password);
        if (!isPasswordValid)
        {
            return Result<GetLoginQueryResult>.Failure( new Error("Email Or Password", "Invalid email or password."));
        }

        var userCreateResult = new UserCreateResult
        (
            user.Id,            
            user.UserName!,
            user.Email!,
            user.FirstName!,
            user.LastName!
        );
        var tokenResult = await jwtService.GenerateTokenAsync(userCreateResult);
        return Result<GetLoginQueryResult>.Success(tokenResult);
    }
}
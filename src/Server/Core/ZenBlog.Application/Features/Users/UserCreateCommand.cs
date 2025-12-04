using System;
using FluentValidation;
using Kommand.Abstractions;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using ZenBlog.Application.Concrete;
using ZenBlog.Domain.Entities;

namespace ZenBlog.Application.Features.Users;

public sealed record UserCreateResult(
    string Id,
    string UserName,
    string Email,
    string FirstName,
    string LastName
);

public sealed record UserCreateCommand(
    string FirstName,
    string LastName,
    string Email,
    string UserName,    
    string Password
) : ICommand<Result<UserCreateResult>>;

public sealed class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

        RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email is required.");

        RuleFor(u => u.UserName)
            .NotEmpty().WithMessage("User name is required.")
            .MaximumLength(50).WithMessage("User name must not exceed 50 characters.");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}

internal sealed class UserCreateCommandHandler(
    UserManager<AppUser> userManager,
    IMapper mapper
) : ICommandHandler<UserCreateCommand, Result<UserCreateResult>>
{
    public async Task<Result<UserCreateResult>> HandleAsync(UserCreateCommand command, CancellationToken cancellationToken)
    {
        var user = mapper.Map<AppUser>(command);
       
     
        var result = await userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new Error("User", e.Description)).ToList();
            return Result<UserCreateResult>.Failure(errors);
        }

        var userCreateResult  = mapper.Map<UserCreateResult>(user);

        return Result<UserCreateResult>.Success(userCreateResult);
        
    }
}
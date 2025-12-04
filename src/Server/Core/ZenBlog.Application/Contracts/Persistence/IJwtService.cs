using System;
using ZenBlog.Application.Features.Users;

namespace ZenBlog.Application.Contracts.Persistence;

public interface IJwtService
{
    Task<GetLoginQueryResult> GenerateTokenAsync(UserCreateResult user);
}

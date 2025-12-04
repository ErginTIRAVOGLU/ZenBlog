using System;

namespace ZenBlog.Application.Contracts.Persistence;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}

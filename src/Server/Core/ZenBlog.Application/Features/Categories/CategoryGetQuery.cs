using System;
using Kommand.Abstractions;
using ZenBlog.Application.Concrete;
using ZenBlog.Domain.Dto;

namespace ZenBlog.Application.Features.Categories;

public sealed record CategoryGetQuery(Guid Id): IQuery<Result<CategoryDto>>;
 
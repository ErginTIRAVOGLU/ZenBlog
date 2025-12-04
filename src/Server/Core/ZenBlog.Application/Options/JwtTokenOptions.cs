using System;

namespace ZenBlog.Application.Options;

public sealed class JwtTokenOptions
{
    public const string SectionName = "JwtTokenOptions";
    public string Issuer { get; set; } = default!; //api.zenblog.com
    public string Audience { get; set; } = default!; //www.zenblog.com
    public string SecretKey { get; set; } = default!;
    public int ExpirationInMinutes { get; set; }
}

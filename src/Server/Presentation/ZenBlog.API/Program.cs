
using ZenBlog.Persistence.Extensions;
using ZenBlog.Application.Extensions;
using ZenBlog.API.Endpoints;
using Mapster;
using MapsterMapper;
using ZenBlog.Application.Mappings;
using Scalar.AspNetCore;
using ZenBlog.API.Handlers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


var config = new TypeAdapterConfig();
config.Scan(typeof(CategoryMapping).Assembly);  

builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddPersistenceServices(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new Microsoft.OpenApi.Models.OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme.",
            In = ParameterLocation.Header,
            Name = "Authorization"
        });
        document.SecurityRequirements = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            }
        };
        return Task.CompletedTask;
    });
});
builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

EndpointsRegistration.MapEndpoints(app);
 

app.Run();

 
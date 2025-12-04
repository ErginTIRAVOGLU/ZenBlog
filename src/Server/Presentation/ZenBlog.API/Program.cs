
using ZenBlog.Persistence.Extensions;
using ZenBlog.Application.Extensions;
using ZenBlog.API.Endpoints;
using Mapster;
using MapsterMapper;
using ZenBlog.Application.Mappings;
using Scalar.AspNetCore;
using ZenBlog.API.Handlers;

var builder = WebApplication.CreateBuilder(args);


var config = new TypeAdapterConfig();
config.Scan(typeof(CategoryMapping).Assembly);  

builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddPersistenceServices(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

EndpointsRegistration.MapEndpoints(app);
 

app.Run();

 
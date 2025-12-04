
using ZenBlog.Persistence.Extensions;
using ZenBlog.Application.Extensions;
using ZenBlog.API.Endpoints;
using Mapster;
using MapsterMapper;
using ZenBlog.Application.Mappings;

var builder = WebApplication.CreateBuilder(args);


var config = new TypeAdapterConfig();
config.Scan(typeof(CategoryMapping).Assembly);  

builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapCategoryEndpoints();
 

app.Run();

 
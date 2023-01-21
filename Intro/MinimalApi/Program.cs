using Microsoft.AspNetCore.Mvc;
using MinimalApi;
using MinimalApi.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<PeopleService>();
builder.Services.AddSingleton<GuidGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapGet("get-example", () => "Hello from GET");
app.MapPost("post-example", () => "Hello from POST");

app.MapMethods("options-or-head", new[] { "HEAD", "OPTIONS" }, () => "Hello of Map Methods");

app.MapGet("get-params/{age:int}", (int age) => $"Age provided was {age}");
app.MapGet("advance-params/{id:regex(^[a-z0-9A-Z]+$)}", (string id) => $"Id was {id}");
app.MapGet("books/{isbn:length(13)}", (string isbn) => $"ISBN is {isbn}");
app.MapGet("people/search", (string? searchTerm, PeopleService peopleService) =>
{
    if (searchTerm is null) return Results.NotFound();

    var results = peopleService.Search(searchTerm!);
    return Results.Ok(results);
});

app.MapGet("mix/{routeParam}", 
(
    [FromRoute]string routeParam,
    [FromQuery(Name = "query")]int queryParam,
    [FromServices]GuidGenerator guidGenerator,
    [FromHeader(Name = "Accept-Encoding")]string? encoding
) => $"{routeParam}:{queryParam}:{guidGenerator}:({encoding})");

app.MapGet("httpcontext-1", async context => await context.Response.WriteAsync($"Hello from http context"));

app.MapGet("http", async (HttpRequest request, HttpResponse response) =>
{
    var queries = request.QueryString.Value;
    await response.WriteAsync($"Hello from HttpResponse. Queries were: {queries}");
});



app.Run();


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();


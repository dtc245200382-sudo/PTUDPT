
var builder = WebApplication.CreateBuilder(args);

// Dang ky dich vu
builder.Services.AddSingleton<GreetingService>();

// Tao ung dung
var app = builder.Build();

// Endpoint 1: Kiem tra API
app.MapGet("/", () => Results.Ok(new
{
    application = "BasicWebApi",
    message = "ASP.NET Core Web API is running."
}));

// Endpoint 2: Chao nguoi dung
app.MapGet("/hello/{name}", (string name, string? language) =>
{
    var message = string.Equals(
        language,
        "en",
        StringComparison.OrdinalIgnoreCase)
        ? $"Hello, {name}!"
        : $"Xin chào, {name}!";

    return Results.Ok(new GreetingResponse(message));
});

// Endpoint 3: Tao loi chao moi
app.MapPost("/greetings",
    (CreateGreetingRequest request, GreetingService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        return Results.BadRequest(
            new ApiError("Name must not be empty."));
    }

    var greeting = service.Create(request.Name.Trim());

    return Results.Created(
        $"/greetings/{greeting.Id}", greeting);
});

// Khoi dong API
app.Run();

// Cac kieu du lieu
public sealed record CreateGreetingRequest(string Name);

public sealed record GreetingResponse(string Message);

public sealed record ApiError(string Message);

public sealed record StoredGreeting(
    int Id,
    string Name,
    DateTimeOffset CreatedAt);

// Dich vu xu ly loi chao
public sealed class GreetingService
{
    private int _lastId;

    public StoredGreeting Create(string name) =>
        new(
            Interlocked.Increment(ref _lastId),
            name,
            DateTimeOffset.UtcNow
        );
}

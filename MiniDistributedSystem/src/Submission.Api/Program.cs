
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Danh sach bai tap mau
var assignments = new[]
{
    new Assignment(1, "Bai tap 1", "Tao API dau tien"),
    new Assignment(2, "Bai tap 2", "Goi API tu client"),
    new Assignment(3, "Bai tap 3", "Xay dung he thong phan tan"),
};

// Endpoint 1: Kiem tra API
app.MapGet("/", () => Results.Ok(new
{
    service = "Submission.Api",
    status = "running"
}));

// Endpoint 2: Lay danh sach bai tap
app.MapGet("/api/assignments", () =>
    Results.Ok(assignments));

// Endpoint 3: Gui thong tin nop bai
app.MapPost("/api/submissions",
    (CreateSubmissionRequest request) =>
{
    if (request.AssignmentId <= 0 ||
        string.IsNullOrWhiteSpace(request.StudentCode) ||
        string.IsNullOrWhiteSpace(request.RepositoryUrl))
    {
        return Results.BadRequest(new
        {
            message = "Du lieu nop bai khong hop le"
        });
    }

    var submission = new SubmissionResult(
        Guid.NewGuid(),
        request.AssignmentId,
        request.StudentCode,
        "received",
        DateTimeOffset.UtcNow
    );

    return Results.Created(
        $"/api/submissions/{submission.Id}",
        submission
    );
});

// Khoi dong API
app.Run();

// Cac kieu du lieu
record Assignment(
    int Id,
    string Title,
    string Description
);

record CreateSubmissionRequest(
    int AssignmentId,
    string StudentCode,
    string RepositoryUrl
);

record SubmissionResult(
    Guid Id,
    int AssignmentId,
    string StudentCode,
    string Status,
    DateTimeOffset ReceivedAt
);

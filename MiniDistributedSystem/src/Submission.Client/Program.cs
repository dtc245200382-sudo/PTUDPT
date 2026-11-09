
using System.Net.Http.Json;

// Dia chi API
var apiBaseUrl = args.Length > 0
    ? args[0]
    : "http://localhost:5000";

// Tao HttpClient
using var http = new HttpClient
{
    BaseAddress = new Uri(apiBaseUrl)
};

// Lay danh sach bai tap tu API
var assignments = await http.GetFromJsonAsync<List<Assignment>>(
    "/api/assignments"
);

Console.WriteLine("Danh sach bai tap:");

foreach (var assignment in assignments ?? new List<Assignment>())
{
    Console.WriteLine($"- {assignment.Id}: {assignment.Title}");
}

// Tao thong tin nop bai
var request = new CreateSubmissionRequest(
    AssignmentId: 1,
    StudentCode: "SV001",
    RepositoryUrl: "https://example.com/sv001/bai-tap-1"
);

// Gui thong tin nop bai len API
var response = await http.PostAsJsonAsync(
    "/api/submissions",
    request
);

// Kiem tra trang thai HTTP
response.EnsureSuccessStatusCode();

// Doc ket qua tu API
var result = await response.Content
    .ReadFromJsonAsync<SubmissionResult>();

Console.WriteLine($"Da nop bai. Ma luot nop: {result?.Id}");
Console.WriteLine($"Trang thai: {result?.Status}");

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

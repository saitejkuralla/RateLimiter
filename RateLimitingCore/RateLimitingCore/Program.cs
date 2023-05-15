using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Intended to limit the rate of incoming requests to a fixed window of time
builder.Services.AddRateLimiter(_ => _
	.AddFixedWindowLimiter(policyName: "fixedwindow", options =>
	{
		options.PermitLimit = 10;
		options.Window = TimeSpan.FromSeconds(10);
		options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
		options.QueueLimit = 3;
	}).RejectionStatusCode=429);

var app = builder.Build();

app.UseAuthorization();

app.UseRateLimiter();
app.MapControllers();

app.Run();

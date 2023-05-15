using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//limit the rate of requests or events by maintaining a fixed number of tokens in a bucket,
builder.Services.AddRateLimiter(_ => _
	.AddTokenBucketLimiter(policyName: "token", options =>
	{
		options.TokenLimit = 2;
		options.ReplenishmentPeriod=TimeSpan.FromSeconds(10);
		options.AutoReplenishment = true;
		options.TokensPerPeriod = 2;
		options.QueueLimit = 0;
		options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
	}).RejectionStatusCode = 429);
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthorization();

app.MapControllers();
//app.MapDefaultControllerRoute().RequireRateLimiting("fixed");
app.Run();

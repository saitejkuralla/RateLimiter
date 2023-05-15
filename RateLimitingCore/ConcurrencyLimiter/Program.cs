using Microsoft.AspNetCore.RateLimiting;
using System.Collections.Generic;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//limit the number of concurrent requests or tasks processed at any given time
builder.Services.AddRateLimiter(_ => _
	.AddConcurrencyLimiter(policyName: "concurrent", options =>
	{
		options.PermitLimit = 2;
		options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
		options.QueueLimit = 0;
	}).RejectionStatusCode = 429);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();

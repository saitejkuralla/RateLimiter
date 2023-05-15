using Microsoft.AspNetCore.RateLimiting;
using System.Collections.Generic;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
//limit the rate of requests or events over a sliding time window. 
builder.Services.AddRateLimiter(_ => _
	.AddSlidingWindowLimiter(policyName: "slidingwindow", options =>
	{
		options.PermitLimit = 1;
		options.Window = TimeSpan.FromSeconds(10);
		options.SegmentsPerWindow = 2;
		options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
		options.QueueLimit = 0;
	}).RejectionStatusCode = 444);


var app = builder.Build();

app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();

app.Run();

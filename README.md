# Rate Limiting
### Why Rate Limiting came into picture?
Users could use up resources in a way that affects others, whether accidentally or on purpose. The server may run out of resources if numerous requests are made for those resources over an extended period of time. Memory, threads, connections, and other finite resources are examples of these resources.To avoid this issue, rate limiter came into picture.
### What is Rate Limiting?
Rate limiting is a technique used to control the rate at which a client can send requests to a server. It is often used to prevent abuse, such as a denial-of-service attack or brute-force attack, and to ensure fair usage of a shared resource.
### Need of Rate Limiting
* Rate-limiting is used to limit network traffic.
* Rate limiting protects our application from denial-of-service attacks (DoS).
* It helps us manage server resources efficiently when a high volume of requests is received.
* Rate limiting helps to improve the performance of our application by reducing unnecessary delays in sending back responses.
* Rate-limiting reduces costs.
## Implementation
* The ```Microsoft.AspNetCore.RateLimiting``` middleware provides rate limiting middleware.So, we need to add it to the application
``` 
app.UseRateLimiter();
```
* The ```RateLimiterOptionsExtensions``` class provides various extension methods for rate limiting. They are:<br>
a. Fixed window<br>
b. Sliding window<br>
c. Concurrency<br>
d. Token bucket<br>
### Fixed Window Limiter
* A fixed window rate limiter is a common technique used to limit the number of requests or events that can be processed within a certain time window. 
* The basic idea is to keep track of the number of requests made during the current time window, and reject any requests that exceed a predefined limit.
* When the time window expires, a new time window starts and the request limit is reset.
* To enable the fixed window limiter, inject the following ```AddRateLimiter``` service in the ```program.cs``` class.
```
builder.Services.AddRateLimiter(_ => _
	.AddFixedWindowLimiter(policyName: "fixedwindow", options =>
	{
		options.PermitLimit = 10;
		options.Window = TimeSpan.FromSeconds(10);
		options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
		options.QueueLimit = 3;
	}).RejectionStatusCode=429);
  ```
  * The above code will call the ```AddRateLimiter``` to add a rate limiting service to the service collection and will further call ```AddFixedWindowLimiter``` method to create a fixed window limiter with a policy name of "fixedwindow" and will set the following options:<br><br>
  a. PermitLimit: The most permits that can be countered up in one timeframe.<br>
  b. Window: specifies the time frame for accepting the requests<br>
  c.QueueProcessingOrder: Determine the order in which requests are processed from a waiting queue<br>
  d. QueueLimit: Sets the maximum number of requests that can be queued while waiting to be processed.<br><br>
* Now, add ```app.UseRateLimiter()``` in program.cs file to enable rate limiting
* Apply the rate limiter with the help of ```EnableRateLimiting``` attribute on controller level or at any endpoint.
### Sliding Window Limiter
* A sliding window rate limiter is a technique used to limit the rate of requests over a sliding time window. 
* In contrast to a fixed window rate limiter, which counts requests within a fixed time interval, a sliding window rate limiter counts requests over a sliding time interval.
* Simply, It adds segments per window. The window slides one segment for each segment interval.
* To enable the sliding window limiter, inject the following ```AddRateLimiter``` service in the ```program.cs``` class.
```
builder.Services.AddRateLimiter(_ => _
	.AddSlidingWindowLimiter(policyName: "slidingwindow", options =>
	{
		options.PermitLimit = 10;
		options.Window = TimeSpan.FromSeconds(10);
    options.SegmentsPerWindow = 2;
		options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
		options.QueueLimit = 3;
	}).RejectionStatusCode=429);
  ```
  * The above code will call the ```AddRateLimiter``` to add a rate limiting service to the service collection and will further call ```AddSlidingWindowLimiter``` method to create a sliding window limiter with a policy name of "slidingwindow" and will set the following options:<br><br>
  a. PermitLimit: The most permits that can be countered up in one timeframe.<br>
  b. Window: specifies the time frame for accepting the requests<br>
  c. SegmentsPerWindow: Determines how many segments the sliding window is divided into.<br>
  d.QueueProcessingOrder: Determine the order in which requests are processed from a waiting queue<br>
  e. QueueLimit: Sets the maximum number of requests that can be queued while waiting to be processed.<br><br>
* Now, add ```app.UseRateLimiter()``` in program.cs file to enable rate limiting
* Apply the rate limiter with the help of ```EnableRateLimiting``` attribute on controller level or at any endpoint.
### Concurrency Limiter
* A concurrency rate limiter is a technique used to limit the number of concurrent requests or tasks that are being processed at any given time.
* The concurrency limiter limits only the number of concurrent requests and doesn’t limit the no of requests in a time period.
* To enable the sliding window limiter, inject the following ```AddRateLimiter``` service in the ```program.cs``` class.
```
builder.Services.AddRateLimiter(_ => _
	.AddSlidingWindowLimiter(policyName: "concurrent", options =>
	{
		options.PermitLimit = 20;
		options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
		options.QueueLimit = 5;
	}).RejectionStatusCode=429);
  ```
  * The above code will call the ```AddRateLimiter``` to add a rate limiting service to the service collection and will further call ```AddConcurrencyLimiter``` method to create a concurrent limiter with a policy name of "concurrent" and will set the following options:<br><br>
  a. PermitLimit: The most permits that can be countered up in one timeframe.<br>
  b.QueueProcessingOrder: Determine the order in which requests are processed from a waiting queue<br>
  c. QueueLimit: Sets the maximum number of requests that can be queued while waiting to be processed.<br><br>
* Now, add ```app.UseRateLimiter()``` in program.cs file to enable rate limiting.
* Apply the rate limiter with the help of ```EnableRateLimiting``` attribute on controller level or at any endpoint.
### Token Bucket Limiter
* A token bucket rate limiter is a technique used to limit the rate of requests or events by maintaining a fixed number of tokens in a bucket, where each token represents a request or event.
* Here, rather than adding back requests taken from expired segment, a fixed number of tokens are added each replenishment period.
* To enable the sliding window limiter, inject the following ```AddRateLimiter``` service in the ```program.cs``` class.
```
builder.Services.AddRateLimiter(_ => _
	.AddTokenBucketLimiter(policyName: "token", options =>
	{
		options.TokenLimit = 10;
		options.ReplenishmentPeriod=TimeSpan.FromSeconds(10);
		options.AutoReplenishment = true;
		options.TokensPerPeriod = 6;
		options.QueueLimit = 2;
		options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
	}).RejectionStatusCode = 429);
  ```
  * The above code will call the ```AddRateLimiter``` to add a rate limiting service to the service collection and will further call ```AddTokenBucketLimiter``` method to create a token bucket limiter with a policy name of "token" and will set the following options:<br><br>
  a. TokenLimit: Maximum number of tokens that can be in the bucket at any time.<br>
  b. ReplenishmentPeriod: Specifies the minimum period between the replenishments.<br>
  c. AutoRelenishment: Specifies whether someone will be calling TryReplenish() to replenish tokens or the TokenBucketRateLimiter is automatically replenishing         tokens.<br>
  d. TokenPerPeriod: For each replenishment, Specifies the maximum number of tokens to restore.<br>
  e.QueueProcessingOrder: Determine the order in which requests are processed from a waiting queue<br>
  f. QueueLimit: Sets the maximum number of requests that can be queued while waiting to be processed.<br><br>
* Now, add ```app.UseRateLimiter()``` in program.cs file to enable rate limiting.
* Apply the rate limiter with the help of ```EnableRateLimiting``` attribute on controller level or at any endpoint.

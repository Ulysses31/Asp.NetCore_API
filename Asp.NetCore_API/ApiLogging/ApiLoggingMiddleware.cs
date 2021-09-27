using Asp.NetCore_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Asp.NetCore_API.ApiLogging
{
	public class ApiLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ApiLoggingMiddleware> _logger;

		public ApiLoggingMiddleware(
			RequestDelegate next,
			ILogger<ApiLoggingMiddleware> logger
		)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext httpContext)
		{
			try {
				var request = httpContext.Request;
				if (request.Path.StartsWithSegments(new PathString("/api"))) {
					var stopWatch = Stopwatch.StartNew();
					var requestTime = DateTime.UtcNow;
					var requestBodyContent = await ReadRequestBody(request);
					var originalBodyStream = httpContext.Response.Body;
					using (var responseBody = new MemoryStream()) {
						var response = httpContext.Response;
						response.Body = responseBody;
						await _next(httpContext);
						stopWatch.Stop();

						string responseBodyContent = null;
						responseBodyContent = await ReadResponseBody(response);
						await responseBody.CopyToAsync(originalBodyStream);

						SafeLog(requestTime,
								stopWatch.ElapsedMilliseconds,
								JsonConvert.SerializeObject(response.Headers),
								response.StatusCode,
								JsonConvert.SerializeObject(request.Headers),
								request.Method,
								request.Path,
								request.QueryString.ToString(),
								requestBodyContent,
								responseBodyContent);
					}
				}
				else {
					await _next(httpContext);
				}
			}
			catch (Exception ex) {
				_logger.LogError(ex.ToString());
				await _next(httpContext);
			}
		}

		private async Task<string> ReadRequestBody(HttpRequest request)
		{
			request.EnableBuffering();

			var buffer = new byte[Convert.ToInt32(request.ContentLength)];
			await request.Body.ReadAsync(buffer, 0, buffer.Length);
			var bodyAsText = Encoding.UTF8.GetString(buffer);
			request.Body.Seek(0, SeekOrigin.Begin);

			return bodyAsText;
		}

		private async Task<string> ReadResponseBody(HttpResponse response)
		{
			response.Body.Seek(0, SeekOrigin.Begin);
			var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
			response.Body.Seek(0, SeekOrigin.Begin);

			return bodyAsText;
		}

		private void SafeLog(
			DateTime requestTime,
			long responseMillis,
			string responseheaders,
			int statusCode,
			string requestheaders,
			string method,
			string path,
			string queryString,
			string requestBody,
			string responseBody
		)
		{
			if (path.ToLower().StartsWith("/api/login")) {
				requestBody = "(Request logging disabled for /api/login)";
				responseBody = "(Response logging disabled for /api/login)";
			}
			//
			//if (requestBody.Length > 100) {
			//	requestBody = $"(Truncated to 100 chars) {requestBody.Substring(0, 100)}";
			//}
			//
			//if (responseBody.Length > 100) {
			//	responseBody = $"(Truncated to 100 chars) {responseBody.Substring(0, 100)}";
			//}
			//
			//if (queryString.Length > 100) {
			//	queryString = $"(Truncated to 100 chars) {queryString.Substring(0, 100)}";
			//}

			var apiResult = JsonConvert.SerializeObject(new CApiLogging() {
				RequestTime = requestTime,
				ResponseMillis = responseMillis,
				ResponseHeaders = responseheaders,
				StatusCode = statusCode,
				RequestHeaders = requestheaders,
				Method = method,
				Path = path,
				QueryString = queryString,
				RequestBody = requestBody,
				ResponseBody = responseBody
			});

			_logger.LogInformation(apiResult);
		}
	}
}
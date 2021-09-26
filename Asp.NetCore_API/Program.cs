using Asp.NetCore_API.Contexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;

namespace Asp.NetCore_API
{
	/// <summary>
	/// Program class
	/// </summary>
	public class Program
	{
		/// <summary>
		/// Defines the entry point of the application.
		/// </summary>
		/// <param name="args">The arguments.</param>
		public static void Main(string[] args)
		{
			// NLOG Conf https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-5
			var logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
			NLog.GlobalDiagnosticsContext.Set("LogDirectory", logPath);

			var host = CreateWebHostBuilder(args).Build();

			// migrate the database.  Best practice = in Main, using service scope
			using (var scope = host.Services.CreateScope())
			{
				try
				{
					var context = scope.ServiceProvider.GetService<LibraryContext>();
					context.Database.Migrate();
				}
				catch (Exception ex)
				{
					var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while migrating the database.");
				}
			}

			// run the web app
			host.Run();
		}

		/// <summary>
		/// Creates the web host builder.
		/// </summary>
		/// <param name="args">The arguments.</param>
		/// <returns>IWebHostBuilder</returns>
		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
				WebHost.CreateDefaultBuilder(args)
						.ConfigureLogging(opt => {
							opt.ClearProviders();
							// Trace Debug Info Warning Error Fatal
							opt.SetMinimumLevel(LogLevel.Trace);
						})
						.UseNLog()
						.UseStartup<Startup>();
	}
}
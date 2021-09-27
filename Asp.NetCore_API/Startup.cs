using Asp.NetCore_API.ApiLogging;
using Asp.NetCore_API.Contexts;
using Asp.NetCore_API.Services;
using Library.API.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Asp.NetCore_API
{
	/// <summary>
	/// Startup class
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// Startup class
		/// </summary>
		/// <param name="configuration"></param>
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		/// <summary>
		/// ConfigureServices class
		/// </summary>
		/// <param name="services"></param>
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<LibraryContext>(options => {
				options.UseSqlServer(Configuration.GetConnectionString("DBConnection"));
			});

			services.AddControllers();

			services.AddApiVersioning(setup => {
				setup.DefaultApiVersion = new ApiVersion(1, 0);
				setup.AssumeDefaultVersionWhenUnspecified = true;
				setup.ReportApiVersions = true;
			});

			services.AddVersionedApiExplorer(setup => {
				setup.GroupNameFormat = "'v'VV";
				setup.SubstituteApiVersionInUrl = true;
			});

			services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

			services.AddSwaggerGen(setupAction => {
				setupAction.AddSecurityDefinition("basicAuth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme() {
					Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
					Scheme = "basic",
					Description = "Input your username and password to access this API."
				});

				setupAction.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement() {
					{
						new OpenApiSecurityScheme() {
							Reference = new OpenApiReference() {
								Type = ReferenceType.SecurityScheme,
								Id = "basicAuth"
							}
						}, new List<string>()
					}
				});

				setupAction.DocInclusionPredicate((documentName, apiDescription) => {
					var actionApiVersionModel = apiDescription.ActionDescriptor
					.GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);

					if (actionApiVersionModel == null) {
						return true;
					}

					if (actionApiVersionModel.DeclaredApiVersions.Any()) {
						return actionApiVersionModel.DeclaredApiVersions.Any(v =>
						$"openapiv{v.ToString()}" == documentName);
					}
					return actionApiVersionModel.ImplementedApiVersions.Any(v =>
							$"openapiv{v.ToString()}" == documentName);
				});

				var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

				setupAction.IncludeXmlComments(xmlCommentsFullPath);
			});

			services.AddMvc(setupAction => {
				setupAction.EnableEndpointRouting = false;
				setupAction.Filters.Add(
										new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
				setupAction.Filters.Add(
						new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));
				setupAction.Filters.Add(
						new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
				setupAction.Filters.Add(
						new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
				setupAction.Filters.Add(
						new ProducesDefaultResponseTypeAttribute());

				setupAction.ReturnHttpNotAcceptable = true;

				// Add Authorization to all controllers
				setupAction.Filters.Add(new AuthorizeFilter());

				// XML support
				setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());

				// var jsonOutputFormatter = setupAction.OutputFormatters
				// 					.OfType<JsonOutputFormatter>().FirstOrDefault();
				//
				// if (jsonOutputFormatter != null)
				// {
				// 	// remove text/json as it isn't the approved media type
				// 	// for working with JSON at API level
				// 	if (jsonOutputFormatter.SupportedMediaTypes.Contains("text/json"))
				// 	{
				// 		jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
				// 	}
				// }
			})
				.AddXmlSerializerFormatters()
				.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

			services.AddAuthentication("Basic")
				.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

			services.AddAutoMapper(typeof(Startup));

			services.AddScoped<IBookRepository, BookRepository>();
			services.AddScoped<IAuthorRepository, AuthorRepository>();
		}

		/// <summary>
		/// Configure class
		/// </summary>
		/// <param name="app"></param>
		/// <param name="env"></param>
		/// <param name="provider"></param>
		// This method gets called by the runtime.
		// Use this method to configure the HTTP request pipeline.
		public void Configure(
			IApplicationBuilder app,
			IWebHostEnvironment env,
			IApiVersionDescriptionProvider provider
		)
		{
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}
			else {
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseSwagger();
			app.UseSwaggerUI(options => {
				options.InjectStylesheet("/Assets/custom-ui.css");
				foreach (var description in provider.ApiVersionDescriptions) {
					options.SwaggerEndpoint(
							$"/swagger/openapi{description.GroupName}/swagger.json",
							description.GroupName.ToUpperInvariant());
					options.RoutePrefix = "";
					//options.EnableDeepLinking();
					options.DisplayOperationId();
				}
			});

			app.UseRouting();

			app.UseAuthentication();
			//app.UseAuthorization();

			app.UseMiddleware<ApiLoggingMiddleware>();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
			});
		}
	}
}
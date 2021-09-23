using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Asp.NetCore_API
{
	/// <summary>
	/// ConfigureSwaggerOptions class
	/// </summary>
	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		private readonly IApiVersionDescriptionProvider provider;

		/// <summary>
		/// ConfigureSwaggerOptions class
		/// </summary>
		/// <param name="provider"></param>
		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
		{
			this.provider = provider;
		}

		/// <summary>
		/// Configure class
		/// </summary>
		/// <param name="options"></param>
		public void Configure(SwaggerGenOptions options)
		{
			// add a swagger document for each discovered API version
			// note: you might choose to skip or document deprecated API versions differently
			foreach (var description in provider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(
					$"openapi{description.GroupName}",
					CreateInfoForApiVersion(description)
				);
			}
		}

		/// <summary>
		/// OpenApiInfo class
		/// </summary>
		/// <param name="description"></param>
		/// <returns>OpenApiInfo</returns>
		private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
		{
			var info = new OpenApiInfo()
			{
				Title = "Sample API",
				Version = description.ApiVersion.ToString(),
				Description = "A sample application with Swagger, Swashbuckle, and API versioning.",
				Contact = new Microsoft.OpenApi.Models.OpenApiContact()
				{
					Email = "info@datacenter.com",
					Name = "Iordanidis Chris",
					Url = new Uri("https://www.linkedin.com/in/iordanidischristos/")
				},
				License = new Microsoft.OpenApi.Models.OpenApiLicense()
				{
					Name = "MIT License",
					Url = new Uri("https://opensource.org/licenses/MIT")
				}
			};

			if (description.IsDeprecated)
			{
				info.Description += " This API version has been deprecated.";
			}

			return info;
		}
	}
}
﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Library.API.Authentication
{
	/// <summary>
	/// BasicAuthenticationHandler class
	/// </summary>
	public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
	{
		public BasicAuthenticationHandler(
				IOptionsMonitor<AuthenticationSchemeOptions> options,
				ILoggerFactory logger,
				UrlEncoder encoder,
				ISystemClock clock)
				: base(options, logger, encoder, clock)
		{
		}

		/// <summary>
		/// Allows derived types to handle authentication.
		/// </summary>
		/// <returns>
		/// The <see cref="T:Microsoft.AspNetCore.Authentication.AuthenticateResult" />.
		/// </returns>
		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (!Request.Headers.ContainsKey("Authorization")) {
				return Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));
			}

			try {
				var authenticationHeader = AuthenticationHeaderValue.Parse(
						Request.Headers["Authorization"]);
				var credentialBytes = Convert.FromBase64String(authenticationHeader.Parameter);
				var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
				var username = credentials[0];
				var password = credentials[1];

				if (username == "admin" && password == "123") {
					var claims = new[] {
						new Claim(ClaimTypes.NameIdentifier, username)
					};
					var identity = new ClaimsIdentity(claims, Scheme.Name);
					var principal = new ClaimsPrincipal(identity);
					var ticket = new AuthenticationTicket(principal, Scheme.Name);

					return Task.FromResult(AuthenticateResult.Success(ticket));
				}
				return Task.FromResult(AuthenticateResult.Fail("Invalid username or password"));
			}
			catch {
				return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header"));
			}
		}
	}
}
using System;

namespace Asp.NetCore_API.Models
{
	/// <summary>
	/// ApiLogging class
	/// </summary>
	public class CApiLogging
	{
		/// <summary>
		/// Gets or sets the request time.
		/// </summary>
		/// <value>
		/// The request time.
		/// </value>
		public DateTime RequestTime { get; set; }

		/// <summary>
		/// Gets or sets the response millis.
		/// </summary>
		/// <value>
		/// The response millis.
		/// </value>
		public long ResponseMillis { get; set; }

		/// <summary>
		/// Gets or sets the response headers.
		/// </summary>
		/// <value>
		/// The response headers.
		/// </value>
		public string ResponseHeaders { get; set; }

		/// <summary>
		/// Gets or sets the status code.
		/// </summary>
		/// <value>
		/// The status code.
		/// </value>
		public int StatusCode { get; set; }

		/// <summary>
		/// Gets or sets the request headers.
		/// </summary>
		/// <value>
		/// The request headers.
		/// </value>
		public string RequestHeaders { get; set; }

		/// <summary>
		/// Gets or sets the method.
		/// </summary>
		/// <value>
		/// The method.
		/// </value>
		public string Method { get; set; }

		/// <summary>
		/// Gets or sets the path.
		/// </summary>
		/// <value>
		/// The path.
		/// </value>
		public string Path { get; set; }

		/// <summary>
		/// Gets or sets the query string.
		/// </summary>
		/// <value>
		/// The query string.
		/// </value>
		public string QueryString { get; set; }

		/// <summary>
		/// Gets or sets the request body.
		/// </summary>
		/// <value>
		/// The request body.
		/// </value>
		public string RequestBody { get; set; }

		/// <summary>
		/// Gets or sets the response body.
		/// </summary>
		/// <value>
		/// The response body.
		/// </value>
		public string ResponseBody { get; set; }
	}
}
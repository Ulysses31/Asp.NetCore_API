using System;

namespace Asp.NetCore_API.Models
{
	/// <summary>
	/// Book class
	/// </summary>
	public class Book
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the first name of the author.
		/// </summary>
		/// <value>
		/// The first name of the author.
		/// </value>
		public string AuthorFirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name of the author.
		/// </summary>
		/// <value>
		/// The last name of the author.
		/// </value>
		public string AuthorLastName { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		public string Description { get; set; }
	}
}
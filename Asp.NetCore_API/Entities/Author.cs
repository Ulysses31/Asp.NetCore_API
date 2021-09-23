using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.NetCore_API.Entities
{
	/// <summary>
	/// Author class
	/// </summary>
	[Table("Authors")]
	public class Author
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>
		/// The first name.
		/// </value>
		[Required]
		[MaxLength(150)]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		/// <value>
		/// The last name.
		/// </value>
		[Required]
		[MaxLength(150)]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the books.
		/// </summary>
		/// <value>
		/// The books.
		/// </value>
		public ICollection<Book> Books { get; set; } = new List<Book>();
	}
}
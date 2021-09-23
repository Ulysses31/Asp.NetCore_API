using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.NetCore_API.Entities
{
	/// <summary>
	/// Book class
	/// </summary>
	[Table("Books")]
	public class Book
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
		/// Gets or sets the title.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		[Required]
		[MaxLength(150)]
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		[MaxLength(2500)]
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the amount of pages.
		/// </summary>
		/// <value>
		/// The amount of pages.
		/// </value>
		public int? AmountOfPages { get; set; }

		/// <summary>
		/// Gets or sets the author identifier.
		/// </summary>
		/// <value>
		/// The author identifier.
		/// </value>
		public Guid AuthorId { get; set; }

		/// <summary>
		/// Gets or sets the author.
		/// </summary>
		/// <value>
		/// The author.
		/// </value>
		public Author Author { get; set; }
	}
}
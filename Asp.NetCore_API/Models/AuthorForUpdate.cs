using System.ComponentModel.DataAnnotations;

namespace Asp.NetCore_API.Models
{
	/// <summary>
	/// AuthorForUpdate class
	/// </summary>
	public class AuthorForUpdate
	{
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
	}
}
namespace Asp.NetCore_API.Models
{
	/// <summary>
	/// BookForCreationWithAmountOfPages class
	/// </summary>
	/// <seealso cref="Asp.NetCore_API.Models.BookForCreation" />
	public class BookForCreationWithAmountOfPages : BookForCreation
	{
		/// <summary>
		/// Gets or sets the amount of pages.
		/// </summary>
		/// <value>
		/// The amount of pages.
		/// </value>
		public int AmountOfPages { get; set; }
	}
}
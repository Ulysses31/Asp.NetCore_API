using Asp.NetCore_API.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asp.NetCore_API.Services
{
	/// <summary>
	/// IBookRepository : IDisposable class
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public interface IBookRepository : IDisposable
	{
		/// <summary>
		/// Gets the books asynchronous.
		/// </summary>
		/// <param name="authorId">The author identifier.</param>
		/// <returns>Task IEnumerable Book</returns>
		Task<IEnumerable<Book>> GetBooksAsync(Guid authorId);

		/// <summary>
		/// Gets the book asynchronous.
		/// </summary>
		/// <param name="authorId">The author identifier.</param>
		/// <param name="bookId">The book identifier.</param>
		/// <returns>Task Book</returns>
		Task<Book> GetBookAsync(Guid authorId, Guid bookId);

		/// <summary>
		/// Adds the book.
		/// </summary>
		/// <param name="bookToAdd">The book to add.</param>
		void AddBook(Book bookToAdd);

		/// <summary>
		/// Saves the changes asynchronous.
		/// </summary>
		/// <returns>Task bool</returns>
		Task<bool> SaveChangesAsync();
	}
}
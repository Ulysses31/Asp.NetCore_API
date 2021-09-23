using Asp.NetCore_API.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asp.NetCore_API.Services
{
	/// <summary>
	/// IAuthorRepository : IDisposable class
	/// </summary>
	/// <seealso cref="System.IDisposable" />
	public interface IAuthorRepository : IDisposable
	{
		/// <summary>
		/// Authors the exists asynchronous.
		/// </summary>
		/// <param name="authorId">The author identifier.</param>
		/// <returns>Task bool</returns>
		Task<bool> AuthorExistsAsync(Guid authorId);

		/// <summary>
		/// Gets the authors asynchronous.
		/// </summary>
		/// <returns>Task IEnumerable Author</returns>
		Task<IEnumerable<Author>> GetAuthorsAsync();

		/// <summary>
		/// Gets the author asynchronous.
		/// </summary>
		/// <param name="authorId">The author identifier.</param>
		/// <returns>Task Author</returns>
		Task<Author> GetAuthorAsync(Guid authorId);

		/// <summary>
		/// Updates the author.
		/// </summary>
		/// <param name="author">The author.</param>
		void UpdateAuthor(Author author);

		/// <summary>
		/// Saves the changes asynchronous.
		/// </summary>
		/// <returns>bool</returns>
		Task<bool> SaveChangesAsync();
	}
}
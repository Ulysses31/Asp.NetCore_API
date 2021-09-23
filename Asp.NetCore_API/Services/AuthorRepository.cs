
using Asp.NetCore_API.Contexts;
using Asp.NetCore_API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asp.NetCore_API.Services
{
	/// <summary>
	/// AuthorRepository class
	/// </summary>
	/// <seealso cref="Asp.NetCore_API.Services.IAuthorRepository" />
	/// <seealso cref="System.IDisposable" />
	public class AuthorRepository : IAuthorRepository, IDisposable
	{
		private LibraryContext _context;

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthorRepository"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public AuthorRepository(LibraryContext context)
		{
			_context = context;
		}

		/// <summary>
		/// Authors the exists asynchronous.
		/// </summary>
		/// <param name="authorId">The author identifier.</param>
		/// <returns>Task bool</returns>
		public async Task<bool> AuthorExistsAsync(Guid authorId)
		{
			return await _context.Authors.AnyAsync(a => a.Id == authorId);
		}

		/// <summary>
		/// Gets the authors asynchronous.
		/// </summary>
		/// <returns>Task IEnumerable Author</returns>
		public async Task<IEnumerable<Author>> GetAuthorsAsync()
		{
			return await _context.Authors.ToListAsync();
		}

		/// <summary>
		/// Gets the author asynchronous.
		/// </summary>
		/// <param name="authorId">The author identifier.</param>
		/// <returns>Task Author</returns>
		/// <exception cref="System.ArgumentException">authorId</exception>
		public async Task<Author> GetAuthorAsync(Guid authorId)
		{
			if (authorId == Guid.Empty)
			{
				throw new ArgumentException(nameof(authorId));
			}

			return await _context.Authors
					.FirstOrDefaultAsync(a => a.Id == authorId);
		}

		/// <summary>
		/// Updates the author.
		/// </summary>
		/// <param name="author">The author.</param>
		public void UpdateAuthor(Author author)
		{
			// no code in this implementation
		}

		/// <summary>
		/// Saves the changes asynchronous.
		/// </summary>
		/// <returns>Task bool</returns>
		public async Task<bool> SaveChangesAsync()
		{
			// return true if 1 or more entities were changed
			return (await _context.SaveChangesAsync() > 0);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_context != null)
				{
					_context.Dispose();
					_context = null;
				}
			}
		}
	}
}
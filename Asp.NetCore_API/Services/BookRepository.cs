using Asp.NetCore_API.Contexts;
using Asp.NetCore_API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCore_API.Services
{
	/// <summary>
	/// BookRepository class
	/// </summary>
	/// <seealso cref="Asp.NetCore_API.Services.IBookRepository" />
	/// <seealso cref="System.IDisposable" />
	public class BookRepository : IBookRepository, IDisposable
	{
		private LibraryContext _context;

		/// <summary>
		/// Initializes a new instance of the <see cref="BookRepository"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <exception cref="System.ArgumentNullException">context</exception>
		public BookRepository(LibraryContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		/// <summary>
		/// Gets the books asynchronous.
		/// </summary>
		/// <param name="authorId">The author identifier.</param>
		/// <returns>Task IEnumerable Book</returns>
		/// <exception cref="System.ArgumentException">authorId</exception>
		public async Task<IEnumerable<Book>> GetBooksAsync(Guid authorId)
		{
			if (authorId == Guid.Empty)
			{
				throw new ArgumentException(nameof(authorId));
			}

			return await _context.Books
					.Include(b => b.Author)
					.Where(b => b.AuthorId == authorId)
					.ToListAsync();
		}

		/// <summary>
		/// Gets the book asynchronous.
		/// </summary>
		/// <param name="authorId">The author identifier.</param>
		/// <param name="bookId">The book identifier.</param>
		/// <returns>Task Book</returns>
		/// <exception cref="System.ArgumentException">
		/// authorId
		/// or
		/// bookId
		/// </exception>
		public async Task<Book> GetBookAsync(Guid authorId, Guid bookId)
		{
			if (authorId == Guid.Empty)
			{
				throw new ArgumentException(nameof(authorId));
			}

			if (bookId == Guid.Empty)
			{
				throw new ArgumentException(nameof(bookId));
			}

			return await _context.Books
					.Include(b => b.Author)
					.Where(b => b.AuthorId == authorId && b.Id == bookId)
					.FirstOrDefaultAsync();
		}

		/// <summary>
		/// Adds the book.
		/// </summary>
		/// <param name="bookToAdd">The book to add.</param>
		/// <exception cref="System.ArgumentNullException">bookToAdd</exception>
		public void AddBook(Book bookToAdd)
		{
			if (bookToAdd == null)
			{
				throw new ArgumentNullException(nameof(bookToAdd));
			}

			_context.Add(bookToAdd);
		}

		/// <summary>
		/// Saves the changes asynchronous.
		/// </summary>
		/// <returns>async Task bool</returns>
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
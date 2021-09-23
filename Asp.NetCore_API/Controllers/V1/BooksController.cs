using Asp.NetCore_API.Models;
using Asp.NetCore_API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCore_API.Controllers.V1
{
	/// <summary>
	/// BooksController : ControllerBase class
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
	// [ApiExplorerSettings(GroupName = "OpenApiBooks")]
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/authors/{authorId}/books")]
	[Produces("application/json", "application/xml")]
	[Consumes("application/json", "application/xml")]
	public class BooksController : ControllerBase
	{
		private readonly IBookRepository _bookRepository;
		private readonly IAuthorRepository _authorRepository;
		private readonly IMapper _mapper;

		/// <summary>
		/// Initializes a new instance of the <see cref="BooksController"/> class.
		/// </summary>
		/// <param name="bookRepository">The book repository.</param>
		/// <param name="authorRepository">The author repository.</param>
		/// <param name="mapper">The mapper.</param>
		public BooksController(
				IBookRepository bookRepository,
				IAuthorRepository authorRepository,
				IMapper mapper)
		{
			_bookRepository = bookRepository;
			_authorRepository = authorRepository;
			_mapper = mapper;
		}

		/// <summary>
		/// Gets the books.
		/// </summary>
		/// <param name="authorId">The author identifier.</param>
		/// <returns>Task ActionResult IEnumerable Book</returns>
		/// <response code="200">Returns list of books</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		[HttpGet()]
		public async Task<ActionResult<IEnumerable<Models.Book>>> GetBooks(
		Guid authorId)
		{
			if (!await _authorRepository.AuthorExistsAsync(authorId))
			{
				return NotFound();
			}

			var booksFromRepo = await _bookRepository.GetBooksAsync(authorId);
			return Ok(_mapper.Map<IEnumerable<Models.Book>>(booksFromRepo));
		}

		/// <summary>
		/// Gets the book.
		/// </summary>
		/// <param name="authorId">The author identifier.</param>
		/// <param name="bookId">The book identifier.</param>
		/// <returns>Task ActionResult Book</returns>
		/// <response code="200">Returns the requested book</response>
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesDefaultResponseType]
		[HttpGet("{bookId}")]
		public async Task<ActionResult<Models.Book>> GetBook(
				Guid authorId,
				Guid bookId)
		{
			if (!await _authorRepository.AuthorExistsAsync(authorId))
			{
				return NotFound();
			}

			var bookFromRepo = await _bookRepository.GetBookAsync(authorId, bookId);
			if (bookFromRepo == null)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<Models.Book>(bookFromRepo));
		}

		/// <summary>
		/// Creates the book.
		/// </summary>
		/// <param name="authorId">The author identifier.</param>
		/// <param name="bookForCreation">The book for creation.</param>
		/// <returns>
		/// Task ActionResult Book
		/// </returns>
		/// <response code="200">Returns the new created book</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesDefaultResponseType]
		[HttpPost()]
		public async Task<ActionResult<Models.Book>> CreateBook(
				Guid authorId,
				[FromBody] BookForCreation bookForCreation)
		{
			if (!await _authorRepository.AuthorExistsAsync(authorId))
			{
				return NotFound();
			}

			var bookToAdd = _mapper.Map<Entities.Book>(bookForCreation);
			_bookRepository.AddBook(bookToAdd);
			await _bookRepository.SaveChangesAsync();

			return CreatedAtRoute(
					"GetBook",
					new { authorId, bookId = bookToAdd.Id },
					_mapper.Map<Models.Book>(bookToAdd));
		}
	}
}

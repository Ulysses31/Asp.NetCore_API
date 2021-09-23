using Asp.NetCore_API.Models;
using Asp.NetCore_API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.NetCore_API.Controllers.V1
{
	/// <summary>
	/// AuthorsController class
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/authors")]
	[Consumes("application/json", "application/xml")]
	[Produces("application/json", "application/xml")]
	public class AuthorsController : Controller
	{
		private readonly IAuthorRepository _authorsRepository;
		private readonly IMapper _mapper;

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthorsController"/> class.
		/// </summary>
		/// <param name="authorsRepository">The authors repository.</param>
		/// <param name="mapper">The mapper.</param>
		public AuthorsController(
				IAuthorRepository authorsRepository,
				IMapper mapper)
		{
			_authorsRepository = authorsRepository;
			_mapper = mapper;
		}


		/// <summary>
		/// Gets the authors.
		/// </summary>
		/// <returns>Task ActionResult IEnumerable Author</returns>
		/// <response code="200">Returns list of authors</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[MapToApiVersion("1.0")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Models.Author>>> GetAuthors()
		{
			var authorsFromRepo = await _authorsRepository.GetAuthorsAsync();
			return Ok(_mapper.Map<IEnumerable<Models.Author>>(authorsFromRepo));
		}

		/// <summary>
		/// Gets the author.
		/// </summary>
		/// <param name="authorId">The author identifier.</param>
		/// <returns>Task ActionResult Author</returns>
		/// <response code="200">Return the author</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[MapToApiVersion("1.0")]
		[HttpGet("{authorId}")]
		public async Task<ActionResult<Models.Author>> GetAuthor(
				Guid authorId
		)
		{
			var authorFromRepo = await _authorsRepository.GetAuthorAsync(authorId);
			if (authorFromRepo == null)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<Models.Author>(authorFromRepo));
		}

		/// <summary>
		/// Updates the author.
		/// </summary>
		/// <param name="authorId">The author identifier.</param>
		/// <param name="authorForUpdate">The author for update.</param>
		/// <returns>Task ActionResult Author></returns>
		/// <remarks>
		/// Sample request (this request updates the author's **first name**) \
		///			PATCH /authors/id
		///			[
		///				{
		///					"op": "replace",
		///					"path": "/firstname",
		///					"value": "new first name"
		///				}
		///			]
		/// </remarks>
		/// <example>
		/// <code>
		///		 var index = 5;
		///		 index++;
		/// </code>
		/// </example>
		/// <response code="200">Returns the updated author</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[MapToApiVersion("1.0")]
		[HttpPut("{authorId}")]
		public async Task<ActionResult<Author>> UpdateAuthor(
				Guid authorId,
				AuthorForUpdate authorForUpdate)
		{
			var authorFromRepo = await _authorsRepository.GetAuthorAsync(authorId);
			if (authorFromRepo == null)
			{
				return NotFound();
			}

			_mapper.Map(authorForUpdate, authorFromRepo);

			//// update & save
			_authorsRepository.UpdateAuthor(authorFromRepo);
			await _authorsRepository.SaveChangesAsync();

			// return the author
			return Ok(_mapper.Map<Models.Author>(authorFromRepo));
		}

		/// <summary>
		/// Updates the author.
		/// </summary>
		/// <param name="authorId">The author identifier.</param>
		/// <param name="patchDocument">The patch document.</param>
		/// <returns>Task ActionResult Author</returns>
		/// <response code="200">Returns the new author</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[HttpPatch("{authorId}")]
		public async Task<ActionResult<Author>> UpdateAuthor(
				Guid authorId,
				JsonPatchDocument<AuthorForUpdate> patchDocument
		)
		{
			var authorFromRepo = await _authorsRepository.GetAuthorAsync(authorId);
			if (authorFromRepo == null)
			{
				return NotFound();
			}

			// map to DTO to apply the patch to
			var author = _mapper.Map<Models.AuthorForUpdate>(authorFromRepo);
			patchDocument.ApplyTo(
				author, 
				(Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState
			);

			// if there are errors when applying the patch the patch doc
			// was badly formed  These aren't caught via the ApiController
			// validation, so we must manually check the modelstate and
			// potentially return these errors.
			if (!ModelState.IsValid)
			{
				return new UnprocessableEntityObjectResult(ModelState);
			}

			// map the applied changes on the DTO back into the entity
			_mapper.Map(author, authorFromRepo);

			// update & save
			_authorsRepository.UpdateAuthor(authorFromRepo);
			await _authorsRepository.SaveChangesAsync();

			// return the author
			return Ok(_mapper.Map<Models.Author>(authorFromRepo));
		}
	}
}

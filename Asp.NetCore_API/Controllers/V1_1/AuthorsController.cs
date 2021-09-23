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

namespace Asp.NetCore_API.Controllers.V1_1
{
	/// <summary>
	/// AuthorsController class
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
	[ApiController]
	[ApiVersion("1.1")]
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
		/// Gets the authors V1.1
		/// </summary>
		/// <returns>Task ActionResult IEnumerable Author</returns>
		/// <response code="200">Returns list of authors</response>
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[MapToApiVersion("1.1")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Models.Author>>> GetAuthors()
		{
			var authorsFromRepo = await _authorsRepository.GetAuthorsAsync();
			return Ok(_mapper.Map<IEnumerable<Models.Author>>(authorsFromRepo));
		}
	}
}

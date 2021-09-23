using AutoMapper;

namespace Asp.NetCore_API.Profiles
{
	/// <summary>
	/// AuthorProfile : Profile class
	/// </summary>
	/// <seealso cref="AutoMapper.Profile" />
	public class AuthorProfile : Profile
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AuthorProfile"/> class.
		/// </summary>
		public AuthorProfile()
		{
			CreateMap<Entities.Author, Models.Author>();
			CreateMap<Models.AuthorForUpdate, Entities.Author>();
		}
	}
}
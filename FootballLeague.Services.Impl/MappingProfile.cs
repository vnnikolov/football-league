using AutoMapper;

namespace FootballLeague.Services.Impl
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Entities.Team, Team>();
			CreateMap<Entities.Match, Match>();
		}
	}
}

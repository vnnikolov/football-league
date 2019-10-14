using AutoMapper;
using FootballLeague.Web.ViewModels.Match;
using FootballLeague.Web.ViewModels.Team;
using SERVICES = FootballLeague.Services;

namespace FootballLeague.Web.ViewModels
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<SERVICES.Team, TeamViewModel>();
			CreateMap<SERVICES.Match, MatchViewModel>();
		}
	}
}

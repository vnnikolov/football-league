using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Web.ViewModels.Team
{
	public class CreateTeamViewModel
	{
		[Required]
		[Display(Name = "Team name")]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "The `{0}` value should be from `{2}` to `{1}` characters long.")]
		public string Name { get; set; }
	}
}

using System;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Web.ViewModels.Match
{
	public class MatchViewModel
	{
		public int Id { get; set; }

		[Required]
		[Display(Name = "Date of match")]
		public DateTime Date { get; set; }

		[Required]
		[Display(Name = "Home team score")]
		public int HomeTeamScore { get; set; }

		[Required]
		[Display(Name = "Away team score")]
		public int AwayTeamScore { get; set; }

		[Required]
		[Display(Name = "Home team")]
		public int HomeTeamId { get; set; }

		[Display(Name = "Home team")]
		public string HomeTeamName { get; set; }

		[Required]
		[Display(Name = "Away team")]
		public int AwayTeamId { get; set; }

		[Display(Name = "Away team")]
		public string AwayTeamName { get; set; }
	}
}

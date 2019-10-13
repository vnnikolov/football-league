using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Web.ViewModels.Match
{
	public class CreateMatchViewModel
	{
		[Required]
		[Display(Name = "Date of match")]
		[DataType(DataType.DateTime)]
		public DateTime? Date { get; set; }

		[Required]
		[Display(Name = "Home team score")]
		public int? HomeTeamScore { get; set; }

		[Required]
		[Display(Name = "Away team score")]
		public int? AwayTeamScore { get; set; }

		[Required]
		[Display(Name = "Home team")]
		public int HomeTeamId { get; set; }

		[Required]
		[Display(Name = "Away team")]
		public int AwayTeamId { get; set; }

		public Dictionary<int, string> Teams { get; set; }
	}
}

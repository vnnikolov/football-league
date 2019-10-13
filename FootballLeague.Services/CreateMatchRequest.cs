using System;

namespace FootballLeague.Services
{
	/// <summary>
	/// Represents a <see cref="Match"/> create request.
	/// </summary>
	public class CreateMatchRequest
	{
		/// <summary>
		/// Gets or sets the date when the match is played.
		/// </summary>
		public DateTime Date { get; set; }

		/// <summary>
		/// Gets or sets the score for home team.
		/// </summary>
		public int HomeTeamScore { get; set; }

		/// <summary>
		/// Gets or sets the score for away team.
		/// </summary>
		public int AwayTeamScore { get; set; }

		/// <summary>
		/// Gets or sets the home team Id.
		/// </summary>
		public int HomeTeamId { get; set; }

		/// <summary>
		/// Gets or sets the away team Id.
		/// </summary>
		public int AwayTeamId { get; set; }
	}
}

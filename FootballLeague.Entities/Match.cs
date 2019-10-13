using System;

namespace FootballLeague.Entities
{
	/// <summary>
	/// Represents a match
	/// </summary>
	public class Match
	{
		/// <summary>
		/// Gets or sets the Id.
		/// </summary>
		public int Id { get; set; }

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
		/// Gets or sets the winner Id
		/// </summary>
		public int? WinnerId { get; set; }


		#region Navigational properties

		/// <summary>
		/// Gets or sets the away team Id.
		/// </summary>
		public int AwayTeamId { get; set; }

		/// <summary>
		/// Gets or sets the away team.
		/// </summary>
		public Team AwayTeam { get; set; }

		/// <summary>
		/// Gets or sets the home team Id.
		/// </summary>
		public int HomeTeamId { get; set; }

		/// <summary>
		/// Gets or sets the home team.
		/// </summary>
		public Team HomeTeam { get; set; }

		#endregion
	}
}

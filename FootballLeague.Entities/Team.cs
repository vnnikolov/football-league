using System.Collections.Generic;

namespace FootballLeague.Entities
{
	/// <summary>
	/// Represents a team.
	/// </summary>
	public partial class Team
	{
		/// <summary>
		/// Gets or sets the Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the total matches count
		/// </summary>
		public int MatchesCount
		{
			get
			{
				return HomeMatches.Count + AwayMatches.Count;
			}
		}

		#region Navigational properties

		/// <summary>
		/// Gets or sets the collection of home matches.
		/// </summary>
		public ICollection<Match> HomeMatches { get; set; }

		/// <summary>
		/// Gets or sets the collection of away matches.
		/// </summary>
		public ICollection<Match> AwayMatches { get; set; }

		/// <summary>
		/// Gets or sets the rank.
		/// </summary>
		public Rank Rank { get; set; }

		#endregion
	}
}

using FootballLeague.Services.Infrastructure;
using System;

namespace FootballLeague.Services
{
	/// <summary>
	/// Represents request for updating match.
	/// </summary>
	public class UpdateMatchRequest : PropertyBag<UpdateMatchRequest>
	{
		/// <summary>
		/// Gets or sets the amatch Id.
		/// </summary>
		public int MatchId { get; set; }

		/// <summary>
		/// Gets or sets the match date.
		/// </summary>
		public DateTime Date
		{
			get => Get<DateTime>();
			set => Set(value);
		}

		/// <summary>
		/// Gets or sets the match date.
		/// </summary>
		public int HomeTeamScore
		{
			get => Get<int>();
			set => Set(value);
		}

		/// <summary>
		/// Gets or sets the match date.
		/// </summary>
		public int AwayTeamScore
		{
			get => Get<int>();
			set => Set(value);
		}
	}
}

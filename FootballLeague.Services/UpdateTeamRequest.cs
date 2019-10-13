using FootballLeague.Services.Infrastructure;

namespace FootballLeague.Services
{
	/// <summary>
	/// Represents request for updating team.
	/// </summary>
	public class UpdateTeamRequest  : PropertyBag<UpdateTeamRequest>
	{
		/// <summary>
		/// Gets or sets the team Id.
		/// </summary>
		public int TeamID { get; set; }

		/// <summary>
		/// Gets or sets the team name.
		/// </summary>
		public string Name
		{
			get => Get<string>();
			set => Set(value);
		}
	}
}

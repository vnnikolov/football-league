using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Services
{
	/// <summary>
	/// Represents a <see cref="Team"/> create request.
	/// </summary>
	public class CreateTeamRequest
	{
		/// <summary>
		/// Team name
		/// </summary>
		public string Name { get; set; }
	}
}

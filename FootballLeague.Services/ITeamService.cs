using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Services
{
	/// <summary>
	/// Represents a service for working with teams.
	/// </summary>
	public interface ITeamService
	{
		/// <summary>
		/// Retrieves all teams.
		/// </summary>
		/// <param name="cancellationToken">the cancellation token</param>
		Task<Team[]> GetTeamsAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Retrieves teams ordered by rank.
		/// </summary>
		/// <param name="cancellationToken">the cancellation token</param>
		Task<Team[]> GetRankedTeamsAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets a team.
		/// </summary>
		/// <param name="id">the ID of the team</param>
		/// <param name="cancellationToken">the cancellation token</param>
		Task<Team> GetTeamAsync(int id, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a team.
		/// </summary>
		/// <param name="request">the data with which the team is created</param>
		/// <param name="cancellationToken">the cancellation token</param>
		Task<int> CreateTeamAsync(CreateTeamRequest request, CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates a team.
		/// </summary>
		/// <param name="id">the data with which the team is updated.</param>
		/// <param name="cancellationToken">the cancellation token</param>
		Task UpdateTeamAsync(UpdateTeamRequest request, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deletes a t eam.
		/// </summary>
		/// <param name="id">the ID of the team to delete</param>
		/// <param name="cancellationToken">the cancellation token</param>
		Task DeleteTeamAsync(int id, CancellationToken cancellationToken = default);
	}
}

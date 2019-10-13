using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Services
{
	/// <summary>
	/// Represents a service for working with matches.
	/// </summary>
	public interface IMatchService
	{
		/// <summary>
		/// Retrieves all matches.
		/// </summary>
		/// <param name="cancellationToken">the cancellation token</param>
		Task<Match[]> GetMatchesAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets a match.
		/// </summary>
		/// <param name="id">the ID of the match</param>
		/// <param name="cancellationToken">the cancellation token</param>
		Task<Match> GetMatchAsync(int id, CancellationToken cancellationToken = default);

		/// <summary>
		/// Creates a match.
		/// </summary>
		/// <param name="request">the data with which the match is created</param>
		/// <param name="cancellationToken">the cancellation token</param>
		Task<int> CreateMatchAsync(CreateMatchRequest request, CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates a match.
		/// </summary>
		/// <param name="id">the data with which the match is updated.</param>
		/// <param name="cancellationToken">the cancellation token</param>
		Task UpdateMatchAsync(UpdateMatchRequest request, CancellationToken cancellationToken = default);

		/// <summary>
		/// Deletes a match.
		/// </summary>
		/// <param name="id">the ID of the match to delete</param>
		/// <param name="cancellationToken">the cancellation token</param>
		Task DeleteMatchAsync(int id, CancellationToken cancellationToken = default);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FootballLeague.Services.Impl
{
	public class MatchServiceImpl : IMatchService
	{
		private readonly Entities.FootballLeagueDbContext _context;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public MatchServiceImpl(
			Entities.FootballLeagueDbContext context,
			ILogger<MatchServiceImpl> logger,
			IMapper mapper)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<Match[]> GetMatchesAsync(
			CancellationToken cancellationToken = default)
		{
			try
			{
				ICollection<Entities.Match> entities = await _context.Matches
					.Include(m => m.HomeTeam)
					.Include(m => m.AwayTeam)
					.ToArrayAsync();

				Match[] Matchs = entities
					.Select(x => _mapper.Map<Entities.Match, Match>(x))
					.ToArray();

				return Matchs;
			}
			catch (ServiceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error retrieving Matchs.");
				throw Conversion.ConvertException(ex);
			}
		}

		public async Task<Match> GetMatchAsync(
			int id,
			CancellationToken cancellationToken = default)
		{
			try
			{
				Entities.Match entity = await _context.Matches
					.Include(m => m.HomeTeam)
					.Include(m => m.AwayTeam)
					.FirstOrDefaultAsync(t => t.Id == id);

				if (entity == null)
				{
					throw new ObjectNotFoundException();
				}

				Match match = _mapper.Map<Entities.Match, Match>(entity);

				return match;
			}
			catch (ServiceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error retrieving Match with id: { id }");
				throw Conversion.ConvertException(ex);
			}
		}

		public async Task<int> CreateMatchAsync(
			CreateMatchRequest request,
			CancellationToken cancellationToken = default)
		{
			try
			{
				if (request == null)
				{
					throw new ArgumentNullException(nameof(request));
				}

				if (request.Date == null)
				{
					throw new ArgumentException("Date cannot be null.");
				}

				if (request.Date > DateTime.Now)
				{
					throw new ArgumentException("Cannot register a match that is not yet played.");
				}

				if (request.HomeTeamId == request.AwayTeamId)
				{
					throw new ArgumentException("Cannot register match between the same team.");
				}

				bool homeTeamsExist = await _context.Teams
					.AnyAsync(t => t.Id == request.HomeTeamId);
				bool awayTeamsExist = await _context.Teams
					.AnyAsync(t => t.Id == request.AwayTeamId);

				if (!homeTeamsExist || !awayTeamsExist)
				{
					throw new ArgumentException("Either one of the teams does not exist in the database.");
				}

				int? winnerID = request.AwayTeamScore > request.HomeTeamScore
						? request.AwayTeamId
						: request.AwayTeamScore < request.HomeTeamScore
							? request.HomeTeamId
							: (int?)null;

				var entity = new Entities.Match
				{
					Date = request.Date,
					AwayTeamId = request.AwayTeamId,
					AwayTeamScore = request.AwayTeamScore,
					HomeTeamId = request.HomeTeamId,
					HomeTeamScore = request.HomeTeamScore,
					WinnerId = winnerID
				};

				_context.Matches.Add(entity);

				await _context.SaveChangesAsync(cancellationToken);

				// Update team rankings
				if (winnerID.HasValue)
				{
					await UpdateTeamRankings(winnerID.Value, 3);
				}
				else
				{
					await UpdateTeamRankings(entity.AwayTeamId, 1);
					await UpdateTeamRankings(entity.HomeTeamId, 1);
				}

				return entity.Id;
			}
			catch (ServiceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating match.");
				throw Conversion.ConvertException(ex);
			}
		}

		public async Task DeleteMatchAsync(
			int id,
			CancellationToken cancellationToken = default)
		{
			Entities.Match entity;

			try
			{
				entity = await _context.Matches
					.FirstOrDefaultAsync(t => t.Id == id);

				if (entity == null)
				{
					throw new ObjectNotFoundException();
				}

				_context.Matches.Remove(entity);

				await _context.SaveChangesAsync(cancellationToken);

				bool wasDraw = !entity.WinnerId.HasValue;
				if (wasDraw)
				{
					await UpdateTeamRankings(entity.AwayTeamId, -1);
					await UpdateTeamRankings(entity.HomeTeamId, -1);
				}
				else
				{
					await UpdateTeamRankings(entity.WinnerId.Value, -3);
				}
			}
			catch (ServiceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erorr deleting match.");
				throw Conversion.ConvertException(ex);
			}
		}

		public async Task UpdateMatchAsync(
			UpdateMatchRequest request,
			CancellationToken cancellationToken = default)
		{
			try
			{
				if (request == null)
				{
					throw new ArgumentNullException(nameof(request));
				}

				Entities.Match entity = await _context.Matches
					.FirstOrDefaultAsync(t => t.Id == request.MatchId);

				if (entity == null)
				{
					throw new ObjectNotFoundException($"Match with Id: { request.MatchId } does not exist.");
				}

				if (request.Date == null)
				{
					throw new ArgumentException("Date cannot be null.");
				}

				if (request.IsSet(x => x.Date))
				{
					if (request.Date > DateTime.Now)
					{
						throw new ArgumentException("Cannot register a match that is not yet played.");
					}

					entity.Date = request.Date;
				}

				if (request.IsSet(x => x.HomeTeamScore))
				{
					entity.HomeTeamScore = request.HomeTeamScore;
				}

				if (request.IsSet(x => x.AwayTeamScore))
				{
					entity.AwayTeamScore = request.AwayTeamScore;
				}

				int? newWinnerID = entity.AwayTeamScore > entity.HomeTeamScore
						? entity.AwayTeamId
						: entity.AwayTeamScore < entity.HomeTeamScore
							? entity.HomeTeamId
							: (int?)null;

				bool hasWinnerChanged = newWinnerID != entity.WinnerId;
				bool wasDraw = !entity.WinnerId.HasValue;
				if (hasWinnerChanged)
				{
					int? loserID = wasDraw
						? (int?)null
						: entity.AwayTeamId != entity.WinnerId
							? entity.AwayTeamId
							: entity.HomeTeamId;

					// If old result was draw, remove 1 point from each team
					if (wasDraw)
					{
						await UpdateTeamRankings(entity.AwayTeamId, -1);
						await UpdateTeamRankings(entity.HomeTeamId, -1);
					}
					// If new result is draw, remove 2 points from old winner and add 1 point to other team, so both earn 1 point
					else if (entity.HomeTeamScore == entity.AwayTeamScore)
					{
						await UpdateTeamRankings(entity.WinnerId.Value, -2);
						await UpdateTeamRankings(loserID.Value, 1);
					}
					// If new result is not draw, remove 3 points from previous winner and add 3 points to new winner
					else
					{
						await UpdateTeamRankings(entity.WinnerId.Value, -3);
						await UpdateTeamRankings(loserID.Value, 3);
					}

					// Set new winner Id
					entity.WinnerId = newWinnerID;
				}

				await _context.SaveChangesAsync(cancellationToken);
			}
			catch (ServiceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating match.");
				throw Conversion.ConvertException(ex);
			}
		}

		private async Task UpdateTeamRankings(
			int teamId,
			int score,
			CancellationToken cancellationToken = default)
		{
			try
			{
				Entities.Rank rank = await _context.Ranks
					.FirstOrDefaultAsync(r => r.TeamId == teamId);

				if (rank == null)
				{
					throw new ArgumentException($"Invalid team Id { teamId }.");
				}

				rank.Points += score;

				await _context.SaveChangesAsync(cancellationToken);
			}
			catch (ServiceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erorr deleting Match.");
				throw Conversion.ConvertException(ex);
			}
		}
	}
}

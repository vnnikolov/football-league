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
	public class TeamServiceImpl : ITeamService
	{
		private readonly Entities.FootballLeagueDbContext _context;
		private readonly ILogger _logger;
		private readonly IMapper _mapper;

		public TeamServiceImpl(
			Entities.FootballLeagueDbContext context,
			ILogger<TeamServiceImpl> logger,
			IMapper mapper)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public async Task<Team[]> GetRankedTeamsAsync(
			CancellationToken cancellationToken = default)
		{
			try
			{
				ICollection<Entities.Team> entities = await _context.Teams
					.Include(t => t.Rank)
					.OrderByDescending(t => t.Rank.Points)
					.ToArrayAsync();

				Team[] teams = entities
					.Select(x => _mapper.Map<Entities.Team, Team>(x, opts =>
					{
						opts.AfterMap((src, dest) => dest.Points = src.Rank.Points);
					}))
					.ToArray();

				return teams;
			}
			catch (ServiceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error retrieving teams.");
				throw Conversion.ConvertException(ex);
			}
		}

		public async Task<Team[]> GetTeamsAsync(
			CancellationToken cancellationToken = default)
		{
			try
			{
				ICollection<Entities.Team> entities = await _context.Teams
					.Include(t => t.Rank)
					.ToArrayAsync();

				Team[] teams = entities
					.Select(x => _mapper.Map<Entities.Team, Team>(x, opts =>
					{
						opts.AfterMap((src, dest) => dest.Points = src.Rank.Points);
					}))
					.ToArray();

				return teams;
			}
			catch (ServiceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error retrieving teams.");
				throw Conversion.ConvertException(ex);
			}
		}

		public async Task<Team> GetTeamAsync(
			int id,
			CancellationToken cancellationToken = default)
		{
			try
			{
				Entities.Team entity = await _context.Teams
					.Include(t => t.Rank)
					.FirstOrDefaultAsync(t => t.Id == id);

				if (entity == null)
				{
					throw new ObjectNotFoundException();
				}

				Team team = _mapper.Map<Entities.Team, Team>(entity, opts =>
				{
					opts.AfterMap((src, dest) => dest.Points = src.Rank.Points);
				});

				return team;
			}
			catch (ServiceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error retrieving team with id: { id }");
				throw Conversion.ConvertException(ex);
			}
		}

		public async Task<int> CreateTeamAsync(
			CreateTeamRequest request,
			CancellationToken cancellationToken = default)
		{
			try
			{
				if (request == null)
				{
					throw new ArgumentNullException(nameof(request));
				}

				if (string.IsNullOrEmpty(request.Name))
				{
					throw new ArgumentException();
				}

				bool isNameTaken = await _context.Teams.AnyAsync(t => t.Name == request.Name);
				if (isNameTaken)
				{
					throw new ArgumentException("There is already team with the same name.");
				}

				var teamEntity = new Entities.Team
				{
					Name = request.Name
				};

				var rankEntity = new Entities.Rank
				{
					// Starting points
					Points = 0,
				};

				teamEntity.Rank = rankEntity;

				_context.Teams.Add(teamEntity);

				await _context.SaveChangesAsync(cancellationToken);

				return teamEntity.Id;
			}
			catch (ServiceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating team.");
				throw Conversion.ConvertException(ex);
			}
		}

		public async Task DeleteTeamAsync(
			int id,
			CancellationToken cancellationToken = default)
		{
			try
			{
				Entities.Team entity = await _context.Teams
					.Include(t => t.HomeMatches)
					.Include(t => t.AwayMatches)
					.FirstOrDefaultAsync(t => t.Id == id);

				if (entity == null)
				{
					throw new ObjectNotFoundException();
				}

				if (entity.MatchesCount > 0)
				{
					throw new InvalidArgumentException("Team that has matches cannot be deleted.");
				}

				_context.Teams.Remove(entity);

				await _context.SaveChangesAsync(cancellationToken);
			}
			catch (ServiceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Erorr deleting team.");
				throw Conversion.ConvertException(ex);
			}
		}

		public async Task UpdateTeamAsync(
			UpdateTeamRequest request,
			CancellationToken cancellationToken = default)
		{
			try
			{
				if (request == null)
				{
					throw new ArgumentNullException(nameof(request));
				}

				Entities.Team entity = await _context.Teams
					.FirstOrDefaultAsync(t => t.Id == request.TeamID);

				if (entity == null)
				{
					throw new ObjectNotFoundException();
				}

				if (request.IsSet(x => x.Name))
				{
					entity.Name = request.Name;
				}

				await _context.SaveChangesAsync(cancellationToken);
			}
			catch (ServiceException)
			{
				throw;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating team.");
				throw Conversion.ConvertException(ex);
			}
		}
	}
}

using AutoMapper;
using FootballLeague.Web.Infrastructure;
using FootballLeague.Web.ViewModels.Match;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SERVICES = FootballLeague.Services;

namespace FootballLeague.Web.Controllers
{
	[Route("matches")]
	public class MatchController : Controller
	{
		private readonly ILogger<MatchController> _logger;
		private readonly IMapper _mapper;
		private readonly SERVICES.IMatchService _matchService;
		private readonly SERVICES.ITeamService _teamService;

		public MatchController(
			ILogger<MatchController> logger,
			IMapper mapper,
			SERVICES.IMatchService matchService,
			SERVICES.ITeamService teamService)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_matchService = matchService ?? throw new ArgumentNullException(nameof(matchService));
			_teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
		}

		[HttpGet]
		[Route("")]
		public async Task<IActionResult> Index(
			CancellationToken cancellationToken = default)
		{
			try
			{
				SERVICES.Match[] matches = await _matchService.GetMatchesAsync();

				var model = new MatchListViewModel();

				model.Matches = matches
					.Select(x => _mapper.Map<SERVICES.Match, MatchViewModel>(x, opts =>
					{
						opts.AfterMap((src, dest) =>
						{
							dest.AwayTeamName = src.AwayTeam.Name;
							dest.HomeTeamName = src.HomeTeam.Name;
						});
					}))
					.ToArray();

				return View(model);
			}
			catch (Exception ex)
			{
				return this.HandleException(ex, message: ex.Message, logger: _logger);
			}
		}

		[HttpGet]
		[Route("{id}")]
		public async Task<IActionResult> Match(
			[FromRoute(Name = "id")]int id,
			CancellationToken cancellationToken = default)
		{
			try
			{
				SERVICES.Match match = await _matchService.GetMatchAsync(id);

				var model = new MatchViewModel();

				model = _mapper.Map<SERVICES.Match, MatchViewModel>(match, opts =>
				{
					opts.AfterMap((src, dest) =>
					{
						dest.AwayTeamName = src.AwayTeam.Name;
						dest.HomeTeamName = src.HomeTeam.Name;
					});
				});

				return View(model);
			}
			catch (Exception ex)
			{
				return this.HandleException(ex, message: ex.Message, logger: _logger);
			}
		}

		[HttpPost]
		[Route("{id}")]
		public async Task<IActionResult> Match(
			[FromRoute(Name = "id")] int matchId,
			MatchViewModel model,
			CancellationToken cancellationToken = default)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}

				var updateRequest = new SERVICES.UpdateMatchRequest
				{
					MatchId = matchId,
					AwayTeamScore = model.AwayTeamScore,
					Date = model.Date,
					HomeTeamScore = model.HomeTeamScore
				};

				await _matchService.UpdateMatchAsync(updateRequest, cancellationToken);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return this.HandleException(ex, message: ex.Message, logger: _logger);
			}
		}

		[HttpGet]
		[Route("create")]
		public async Task<IActionResult> Create()
		{
			try
			{
				SERVICES.Team[] teams = await _teamService.GetTeamsAsync();

				var model = new CreateMatchViewModel
				{
					Date = null,
					AwayTeamScore = null,
					HomeTeamScore = null,
					Teams = teams.ToDictionary(t => t.Id, t => t.Name),
				};

				return View(model);
			}
			catch (Exception ex)
			{
				return this.HandleException(ex, message: ex.Message, logger: _logger);
			}
		}

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> Create(
			CreateMatchViewModel model,
			CancellationToken cancellationToken = default)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}

				if (model.Date > DateTime.Now)
				{
					ModelState.AddModelError(nameof(model.Date), "Match date cannot be in the future.");
					return View(model);
				}
				if (model.HomeTeamId == model.AwayTeamId)
				{
					ModelState.AddModelError(nameof(model.HomeTeamId), "A team cannot have match with itself.");
					return View(model);
				}

				var createRequest = new SERVICES.CreateMatchRequest
				{
					AwayTeamId = model.AwayTeamId,
					AwayTeamScore = model.AwayTeamScore.Value,
					Date = model.Date.Value,
					HomeTeamId = model.HomeTeamId,
					HomeTeamScore = model.HomeTeamScore.Value
				};

				int id = await _matchService.CreateMatchAsync(createRequest, cancellationToken);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return this.HandleException(ex, message: ex.Message, logger: _logger);
			}
		}

		[HttpPost]
		[Route("{id}/delete")]
		public async Task<IActionResult> Delete(
			[FromRoute(Name = "id")] int id,
			CancellationToken cancellationToken = default)
		{
			try
			{
				await _matchService.DeleteMatchAsync(id, cancellationToken);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return this.HandleException(ex, message: ex.Message, logger: _logger);
			}
		}
	}
}

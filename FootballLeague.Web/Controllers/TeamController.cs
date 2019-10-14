using AutoMapper;
using FootballLeague.Web.Infrastructure;
using FootballLeague.Web.ViewModels.Team;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SERVICES = FootballLeague.Services;

namespace FootballLeague.Web.Controllers
{
	[Route("teams")]
	public class TeamController : Controller
	{
		private readonly ILogger<TeamController> _logger;
		private readonly IMapper _mapper;
		private readonly SERVICES.ITeamService _teamService;

		public TeamController(
			ILogger<TeamController> logger,
			IMapper mapper,
			SERVICES.ITeamService teamService)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
		}

		[HttpGet]
		[Route("")]
		public async Task<IActionResult> Index(
			CancellationToken cancellationToken = default)
		{
			try
			{
				SERVICES.Team[] teams = await _teamService.GetRankedTeamsAsync();

				var model = new TeamListViewModel();

				model.Teams = teams
					.Select(x => _mapper.Map<TeamViewModel>(x))
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
		public async Task<IActionResult> Team(
			[FromRoute(Name = "id")]int id,
			CancellationToken cancellationToken = default)
		{
			try
			{
				SERVICES.Team team = await _teamService.GetTeamAsync(id);

				var model = new TeamViewModel();

				model = _mapper.Map<TeamViewModel>(team);

				return View(model);
			}
			catch (Exception ex)
			{
				return this.HandleException(ex, message: ex.Message, logger: _logger);
			}
		}

		[HttpPost]
		[Route("{id}")]
		public async Task<IActionResult> Team(
			[FromRoute(Name = "id")] int teamId,
			TeamViewModel model,
			CancellationToken cancellationToken = default)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}

				var updateRequest = new SERVICES.UpdateTeamRequest
				{
					TeamID = teamId,
					Name = model.Name
				};

				await _teamService.UpdateTeamAsync(updateRequest, cancellationToken);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return this.HandleException(ex, message: ex.Message, logger: _logger);
			}
		}

		[HttpGet]
		[Route("create")]
		public IActionResult Create()
		{
			var model = new CreateTeamViewModel();

			return View(model);
		}

		[HttpPost]
		[Route("create")]
		public async Task<IActionResult> Create(
			CreateTeamViewModel model,
			CancellationToken cancellationToken = default)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(model);
				}

				var createRequest = new SERVICES.CreateTeamRequest
				{
					Name = model.Name
				};

				int id = await _teamService.CreateTeamAsync(createRequest, cancellationToken);

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
				await _teamService.DeleteTeamAsync(id, cancellationToken);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				return this.HandleException(ex, message: ex.Message, logger: _logger);
			}
		}
	}
}

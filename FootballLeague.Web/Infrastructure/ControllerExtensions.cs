using FootballLeague.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using SERVICES = FootballLeague.Services;

namespace FootballLeague.Web.Infrastructure
{
	public static class ControllerExtensions
	{
		/// <summary>
		/// Handles an exception on a controller by returning a corresponding result.
		/// </summary>
		/// <param name="controller">controller the exception has occurred in</param>
		/// <param name="exception">exception being handled</param>
		/// <param name="message">message</param>
		/// <param name="logger">logger to use for logging</param>
		/// <param name="eventId">event Id to use for logging</param>
		/// <returns>action result to return (or re-throws the exception)</returns>
		public static IActionResult HandleException(this Controller controller,
			Exception exception,
			string message = null,
			ILogger logger = null,
			int eventId = default)
		{
			if (controller == null)
			{
				throw new ArgumentNullException(nameof(controller));
			}

			if (exception == null)
			{
				throw new ArgumentNullException(nameof(exception));
			}

			IActionResult result;

			switch (exception)
			{
				case SERVICES.MissingArgumentException ex1:
				case SERVICES.InvalidArgumentException ex2:
					{
						message = message ?? "No argument or an invalid argument was specified to a service.";

						logger?.LogWarning(eventId, exception, message);

						result = controller.BadRequest();
						break;
					}
				case SERVICES.ObjectNotFoundException ex:
					{
						message = message ?? "An object requested from a service could not be found.";

						logger?.LogWarning(eventId, exception, message);

						result = controller.NotFound();
						break;
					}
				case FormatException ex:
					{
						message = message ?? "An argument with bad format was provided.";

						logger?.LogWarning(eventId, exception, message);

						result = controller.BadRequest();
						break;
					}
				case SERVICES.InternalErrorException ex:
					{
						message = message ?? "An internal error has occurred while processing a request to a service.";

						logger?.LogError(eventId, exception, message);

						result = controller.StatusCode(500);
						break;
					}
				case SERVICES.ServiceException ex:
					{
						message = message ?? "An unknown error has occurred while processing a request to service.";

						logger?.LogError(eventId, exception, message);

						result = controller.StatusCode(500);
						break;
					}
				default:
					{
						message = message ?? "An unknown error has occurred while processing a user request.";

						logger?.LogError(eventId, exception, message);

						// this should be handled by a generic handler (error page)
						throw exception;
					}
			}

			return result;
		}
	}
}

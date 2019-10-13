using System;

namespace FootballLeague.Services.Impl
{
	public static class Conversion
	{
		public static ServiceException ConvertException(Exception ex)
		{
			ServiceException result;

			switch (ex)
			{
				case ArgumentNullException argumentNullException:
					{
						result = new MissingArgumentException(
							message: "Request is missing argument.",
							innerException: argumentNullException);
						break;
					}
				case ArgumentException argumentException:
					{
						result = new InvalidArgumentException(
							message: "Request has an invalid argument.",
							innerException: argumentException);
						break;
					}
				case ServiceException serviceException:
					{
						result = serviceException;
						break;
					}
				default:
					{
						result = new InternalErrorException(
							message: "An internal error has occurred.",
							innerException: ex);
						break;
					}
			}

			return result;
		}
	}
}

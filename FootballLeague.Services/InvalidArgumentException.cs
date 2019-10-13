using System;

namespace FootballLeague.Services
{
	public class InvalidArgumentException : ServiceException
	{
		public InvalidArgumentException()
		{
		}

		public InvalidArgumentException(string message)
			: base(message)
		{
		}

		public InvalidArgumentException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}

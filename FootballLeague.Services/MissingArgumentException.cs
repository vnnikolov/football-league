using System;

namespace FootballLeague.Services
{
	public class MissingArgumentException : ServiceException
	{
		public MissingArgumentException()
		{
		}

		public MissingArgumentException(string message)
			: base(message)
		{
		}

		public MissingArgumentException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}

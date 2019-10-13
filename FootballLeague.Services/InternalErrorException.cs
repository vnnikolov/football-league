using System;

namespace FootballLeague.Services
{
	public class InternalErrorException : ServiceException
	{
		public InternalErrorException()
		{
		}

		public InternalErrorException(string message)
			: base(message)
		{
		}

		public InternalErrorException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}

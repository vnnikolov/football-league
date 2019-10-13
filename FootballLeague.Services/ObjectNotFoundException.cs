using System;

namespace FootballLeague.Services
{
	public class ObjectNotFoundException : ServiceException
	{
		public ObjectNotFoundException()
		{
		}

		public ObjectNotFoundException(string message)
			: base(message)
		{
		}

		public ObjectNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}

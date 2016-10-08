using System;

namespace CS422
{
	public abstract class WebService
	{		
		public abstract void Handler(WebRequest req);

		/// <summary>
		/// Gets the service URI. This is a string of the form:
		/// /MyServiceName.whatever
		/// If a request hits the server and the request target starts with this
		/// string then it will be routed to this service to handle.
		/// </summary>
		public abstract string ServiceURI
		{
			get;
		}
	}
}


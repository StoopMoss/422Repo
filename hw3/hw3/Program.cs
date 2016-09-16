using System;
using CS422;

namespace hw3
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			
			bool result = WebServer.Start (4220, responseTemplate: "HTTP/1.1 200 OK\r\n" +
				"Content-Type: text/html\r\n" +
				"\r\n\r\n" +
				"<html>ID Number: {0}<br>" +
				"DateTime.Now: {1}<br>" +
				"Requested URL: {2}</html>");

			Console.WriteLine ("result was: "+ result.ToString());
			
		}
	}
}

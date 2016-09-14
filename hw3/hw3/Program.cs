using System;
using CS422;

namespace hw3
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			
			WebServer.Start (4220, responseTemplate: "");
			
		}
	}
}

using System;

namespace CS422
{
	public class DemoService: WebService
	{		
    private const string c_template =
    "<html>This is the response to the request:<br>" +
    "Method: {0}<br>Request-Target/URI: {1}<br>" +
    "Request body size, in bytes: {2}<br><br>" +
    "Student ID: {3}</html>";

    private WebRequest _request;
    private string _studentId = "11282717";

		public override string ServiceURI
		{
			get {return "/"; }
		}
		
    public override void Handler(WebRequest request)
    {
      _request = request;
      // cREATE response
      string response = FormatResponseTemplate(c_template);
      
      // Write response
      Console.WriteLine("About to Write to the networkStream");
      bool writeSuccessful = request.WriteHTMLResponse(response);
      Console.WriteLine("DemoService Response Write status: " + writeSuccessful);
    }

    public string FormatResponseTemplate(string template)
		{
			// string response = string.Format(template, _request.Method, _request.URI,
      // _request.Body.Length, _studentId);

      string response = string.Format(template, _request.Method, _request.URI,
      template.Length, _studentId);

      return response;
		}


	}
}

using System.IO;
using System;

namespace CS422
{
	public class NumberedTextWriter  : System.IO.TextWriter
	{
		private int _currentLineNumber;
		private TextWriter _writer;

		public override  System.Text.Encoding Encoding
		{
			get{ return _writer.Encoding; }           
		}


		//Constructors 
		public NumberedTextWriter(TextWriter writer)
		{
			_writer = writer;
			_currentLineNumber = 1;
		}

		public NumberedTextWriter(TextWriter writer, int startingLineNumber)
		{
			_writer = writer;
			_currentLineNumber = startingLineNumber;
		}

		//Methods
		public override void WriteLine(string _string)
		{
			
			string s;
			// Add current line number to start of string
			s = _currentLineNumber.ToString() + ": " + _string;

			// then pass to WriteLine on TextWriter member
			_writer.WriteLine(s);

			// Increment Current line member
			_currentLineNumber++; 
		}			
	}
}
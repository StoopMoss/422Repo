using System;
using System.IO;


namespace CS422
{
	public class ConcatStream : Stream
	{
		//Members
		private long _position;
		private long _length;
		private bool _canSeek;
		private bool _canRead;
		private bool _canWrite;

		private bool _lengthSupport;
		private bool _usedFirstConstructor;
		private bool _usedSecondConstructor;

		private MemoryStream _stream1;
		private MemoryStream _stream2;


		////////////
		//Properties
		public override long Position
		{
			get
			{
				return _position;
			}

			set
			{
				if (value >= 0 && value <= _length)
				{
					_position = value;	
				}
				else if (value < 0)
				{
					_position = 0;	
				}
				else
				{
					_position = _length;
				}
			}
		}

		public override bool CanSeek
		{
			get{ return _canSeek;}
		}

		public override bool CanRead
		{ 
			get{ return _canRead;}
		}

		public override bool CanWrite
		{
			get{ return _canWrite;}
		}

		public override long Length
		{
			get 
			{
				 return _length;
			} 
		}

		public bool LengthSupport
		{
			get { return _lengthSupport;} 
			set { _lengthSupport = value;} 
		}


		///////////////
		//Constructors 
		public ConcatStream(Stream first, Stream second)
		{
			long firstStreamLength = 0;
			long secondStreamLength = 0;

			// Check first stream's Length Support
			try
			{
				firstStreamLength = first.Length;
			}
			catch (NotSupportedException)
			{				
				throw new ArgumentException();
			}
			// Check second stream's Length Support
			try
			{
				secondStreamLength = second.Length;
			}
			catch (NotSupportedException)
			{				
				LengthSupport = false;
			}			

			if (secondStreamLength != 0)
			{
				LengthSupport = true;
			}
		
			SetProperties(first, second);
			_usedFirstConstructor = true;
			Position = 0;

			//Concat streams
			// _stream1 = new MemoryStream();
			// first.CopyTo( _stream);
			// second.CopyTo(_stream);
			//Length = _stream.Length;
												
		}

		public ConcatStream(Stream first, Stream second, long fixedLength)
		{
			long firstStreamLength = 0;

			// Check first stream's Length Support
			try
			{
				firstStreamLength = first.Length;
				if (fixedLength < firstStreamLength)
				{
					throw new ArgumentException(
						"fixedLength was less than first stream's length");	
				}
			}
			catch (NotSupportedException)
			{				
				throw new ArgumentException();
			}
			
			SetProperties(first, second);
			LengthSupport = true;
			_length = fixedLength;			
			_usedSecondConstructor = true;
			Position = 0;

			//Concat streams
			// _stream1 = new MemoryStream();
			// first.CopyTo( _stream);
			// second.CopyTo(_stream);
			// //Length = fixedLength;
			
				
		}

	

		//////////
		//Methods	
		public override void Flush()
		{
			throw new NotImplementedException ();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException ();
		}

		public override void SetLength (long value)
		{
			if (value < 0) {
				_length = 0;					
			} else {
				_length = value;
			}
		}

		public override void Write(byte[] byteArray, int offset, int count)
		{
			throw new NotImplementedException ();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			// If the second stream doesn’t support seeking, 
			// provide forward-only reading functionality with no seeking
			// if (CanSeek)
			// { blah }
			return 0;
		}

		//////////////////
		//Utility Methods

		// Sets the capabilities of the concat stream to 
		// the lesser capabilities of the combined streams
		public void SetProperties(Stream first, Stream second)
		{
			//Set CanRead
			if (first.CanRead && second.CanRead){
				_canRead = true;
			}else{
				_canRead = false;
			}

			//Set CanRead
			if (first.CanWrite && second.CanWrite){
				_canWrite = true;
			}else{
				_canWrite = false;
			}

			//Set CanRead
			if (first.CanSeek && second.CanSeek){
				_canSeek = true;
			}else{
				_canSeek = false;
			}
		}
	}
}


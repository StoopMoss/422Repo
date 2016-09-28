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

		private MemoryStream _stream;


		///////////////
		//Constructors 
		public ConcatStream(Stream first, Stream second)
		{
			SetProperties(first, second);

			//TODO: figure out the right way to see if Length is supported
			//Set LengthSupport
			if (first.Length != 0 && second.Length  != 0) 
			{
				this.LengthSupport = true;
			}
			else if(first.Length != 0)
			{
				throw new Exception ("First stream does not support length");
			}

			//Concat streams
			_stream = new MemoryStream();
			first.CopyTo( _stream);
			second.CopyTo(_stream);
			Position = 0;
			//Length = _stream.Length;
			_usedFirstConstructor = true;
												
		}

		public ConcatStream(Stream first, Stream second, long fixedLength)
		{
			SetProperties(first, second);

			//TODO: figure out the right way to see if Length is supported
			//Set LengthSupport
			if(first.Length != 0)
			{
				throw new Exception ("First stream does not support length");
			}
			else{
				LengthSupport = true;
			}

			//Concat streams
			_stream = new MemoryStream();
			first.CopyTo( _stream);
			second.CopyTo(_stream);
			Position = 0;
			//Length = fixedLength;
			_usedFirstConstructor = false;
				
		}


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
			get { return _length;} 
		}

		public bool LengthSupport
		{
			get { return _lengthSupport;} 
			set { _lengthSupport = value;} 
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
			return 0;
		}

		//////////////////
		//Utility Methods
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


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


		//
		//Constructors 
		//
		public ConcatStream(Stream first, Stream second){}
		public ConcatStream(Stream first, Stream second, long fixedLength){}


		//
		//Properties
		//
		public override bool CanSeek{ get{ return _canSeek;}}
		public override bool CanRead{ get{ return _canRead;}}
		public override bool CanWrite{ get{ return _canWrite;}}
		public override long Length{ get { return _length;} }


		//
		//Methods
		//
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
		}
	}
}


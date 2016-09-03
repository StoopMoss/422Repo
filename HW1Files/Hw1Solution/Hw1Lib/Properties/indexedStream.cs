using System;
using System.IO;

namespace CS422
{
	public class IndexedNumsStream : System.IO.Stream
	{
		private long _position;
		private long _length;

		private bool _canRead;
		private bool _canWrite;

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

		public override bool CanSeek{ get{ return true;}}
		public override bool CanRead{ get{ return true;}}
		public override bool CanWrite{ get{ return false;}}
		public override long Length{ get { return _length;} }

		//Constructor(s)
		public IndexedNumsStream()
		{
			_length = 255;
			_position = 0;
		}

		public IndexedNumsStream(long length)
		{
			//length < 0 ? _length = 0 : _length = length;
			if (length < 0) {
				_length = 0;					
			} else {
				_length = length;
			}
			Position = 0;
		}

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
			throw new NotImplementedException ();
		}

		public override void Write(byte[] byteArray, int offset, int count)
		{
			throw new NotImplementedException ();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			long bytes_read = 0;
			long position_marker = 0;;
			// Check Bounds
			if (offset < 0) {
				Console.WriteLine ("Stream Read Error: offset must be positive");
				Console.WriteLine ();
				return -1;
			}

			position_marker = Position;
			Console.WriteLine ("Reading stream...");

			for (int i = 0; i < count; i++)
			{				
				if (position_marker > _length)
				{
					Console.WriteLine ("End of stream: PostionMarker " + position_marker.ToString());
					Console.WriteLine ();
					return -1;
				}
				buffer [i] = (byte)(Position % 256);
				Console.WriteLine (buffer[i].ToString());
				Position++;
				position_marker++;
				bytes_read++;
			}
			Console.WriteLine ();
			return 0;
		}
	}
}


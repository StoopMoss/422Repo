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
		private bool _fixedLength;
		//private bool _usedSecondConstructor;

		private Stream _stream1;
		private Stream _stream2;


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
			long secondStreamLength = -1;

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

			if (secondStreamLength != -1)
			{
				LengthSupport = true;
				_length = firstStreamLength + secondStreamLength;
			}
		
			SetProperties(first, second);
			_fixedLength = false;
			Position = 0;

			_stream1 = first;
			_stream2 = second;
												
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
			_fixedLength = true;
			Position = 0;

			_stream1 = first;
			_stream2 = second;
		}

	

		//////////
		//Methods	
		public override void Flush()
		{
			throw new NotImplementedException ();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			if(this.CanSeek)
			{
				//Seek 
			}
			else
			{
				throw new NotSupportedException ();
			}
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

		public override int Read(byte[] buffer, int offset, int count)
		{
			int bytesRead = 0;
			int i = 0;
			int bytesToReadFromS2 = 0;

			Console.WriteLine("In Read: Position = " + Position.ToString() );

			// see which stream to start reading from 
			if (this.Position < _stream1.Length) // Start in stream 1
			{				
				if (count > _stream1.Length)
				{
					// will have to read over the boundry of the two streams
					// So read all of stream1 
					bytesRead = _stream1.Read(buffer, offset, (int)_stream1.Length);
					this.Position += bytesRead;
					bytesToReadFromS2 =  count - bytesRead;
				}
				else
				{
					// will Not have to read over the boundry of the two streams
					// So read all of count from stream1 and return 
					bytesRead = _stream1.Read(buffer, offset, count);
					this.Position += bytesRead;
					return bytesRead;
				}
				
				// At this point we have read stream1 completly and will read the remainder
				// of count from stream2
				bytesRead += _stream2.Read(buffer, offset + bytesRead, bytesToReadFromS2);
				this.Position += bytesRead;
				return bytesRead;
			}

			// Concat position was located in stream2 so read from stream two and return
			// Read all of stream two and return 
			bytesRead = _stream2.Read(buffer, offset, count);
			this.Position += bytesRead;
			return bytesRead;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			int bytesToWriteToS2 = 0;
			int offsetForS2 = 0;
			//
			if (this.Position < _stream1.Length) // Then start writing in stream1
			{
				if (count > _stream1.Length) //  see if you end of stream1 will be reached
				{
					// only write to end of stream1 then continue with stream2
					Console.WriteLine("1 Writing "+ _stream1.Length.ToString() + "bytes to stream1");
					_stream1.Write(buffer, offset, (int)_stream1.Length);
					this.Position += (int)_stream1.Length;
					bytesToWriteToS2 = count - (int)_stream1.Length;
					offsetForS2 = offset + (int)_stream1.Length;
										
					try
					{
						Console.WriteLine("2 Writing "+ bytesToWriteToS2.ToString() + "bytes to stream2");
						_stream2.Write(buffer,offsetForS2,bytesToWriteToS2);
						this.Position += bytesToWriteToS2;
						return;
					}
					catch(NotSupportedException)
					{
						throw new NotSupportedException("Concat stream is not expandable");						
					}
				}
				else // end of stream1 will not be reached
				{
					Console.WriteLine("3 Writing "+ count.ToString() + "bytes to stream1");
					_stream1.Write(buffer,offset,count);
					this.Position += count;					
				}
			}
			else // Start writing in stream2
			{
				if(CanSeek)// seeking in stream2 requires LengthSupport
				{
						_stream2.Position = this.Position - (int)_stream1.Length;
						try
						{
							Console.WriteLine("4 Writing "+ count.ToString() + "bytes to stream2");
							_stream2.Write(buffer,offset,count);
							this.Position += count;
						}
						catch(NotSupportedException)
						{
							throw new NotSupportedException("Concat stream is not expandable");				
						}
				}
				else
				{
					// check to see if at exact position in stream2
					if(this.Position == _stream1.Length + _stream2.Position) // off by one ? 
					{
						try
						{
							_stream2.Write(buffer,offset,count);
							this.Position += count;
						}
						catch(NotSupportedException)
						{
							throw new NotSupportedException("Concat stream is not expandable");				
						}
					}
					else
					{
						throw new NotSupportedException("Cannot write because Seek is not supported");
					}
				}
			}
		}

		//////////////////
		//Utility Methods
		// Should all be private in production
		// public for testing
		
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


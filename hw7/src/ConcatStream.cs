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
		//private bool _fixedLength;
		//private bool _usedSecondConstructor;

		private Stream _stream1;
		private Stream _stream2;


		////////////
		//Properties
		public override long Position
		{
			get
			{
				if (CanSeek){
					return _position;
        }else {
					throw new NotSupportedException ();
        }
			}

			set
			{
        if (CanSeek)
        {
          if (LengthSupport)
          {
            if (value >= 0 && value <= _length)
            {
              _position = value;
            }
            else if (value > _length)
            {
              _position = _length;  
            }
          }
          else
          {
            if (value >= 0)
				    {
  						_position = value;
  				  }
  				  else if (value < 0)
  				  {
    					_position = 0;	
  				  }
          }         
        }
        else
        {
          throw new NotSupportedException();
        }
			}
		}

		public override bool CanSeek
		{
			get{ return _canSeek;}

		}
		
		//for testing... 
		public bool SetSeek
		{
			set{_canSeek = value;}
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
				if (LengthSupport == true)
				{
					return _length;
				}
				else
				{
					throw new NotSupportedException("Length not supported");
				}				
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
        Console.WriteLine("firstConstructor: second try statement");
				secondStreamLength = second.Length;
			}
			catch (NotSupportedException)
			{				
        Console.WriteLine("firstConstructor: in second catch");
				LengthSupport = false;
			}			

			if (secondStreamLength != -1)
			{
        Console.WriteLine("firstConstructor: in that if");
				LengthSupport = true;
				SetLength(firstStreamLength + secondStreamLength);
			}
		
			SetProperties(first, second);
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
        Console.WriteLine("Second Constructor try");
				firstStreamLength = first.Length;
        if (first.Length == null)
        {
          Console.WriteLine("null");
          throw new ArgumentException(
            "ConcatStreamFixedlengthConstructor: no length on first stream");
        }
				if (fixedLength < firstStreamLength)
				{
          Console.WriteLine("blah");
					throw new ArgumentException(
						"fixedLength was less than first stream's length");	
				}
			}
			catch (NotSupportedException)
			{				
        Console.WriteLine("Second Constructor catch");
				throw new ArgumentException();
			}
			
			SetProperties(first, second);
			LengthSupport = true;
			SetLength(fixedLength);			
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

      // if(LengthSupport)
      // {
      //   if (count > Length) 
      //   {
      //     // Make sure to not read over the stream boundry
      //     count = (int)Length;
      //   }
      // }

			// Console.WriteLine("In Read: Position = " + Position.ToString() );
			// Console.WriteLine("In Read:_stream1.length = " + _stream1.Length.ToString() );
			// Console.WriteLine("In Read: count = " + count.ToString() );
			// see which stream to start reading from 
			if (this.Position < _stream1.Length) // Start in stream 1
			{				
        Console.WriteLine("In Read: first if");
				if (count > _stream1.Length)
				{
					// will have to read over the boundry of the two streams
					// So read all of stream1 
				  Console.WriteLine("Read 1 ");		
					bytesRead = _stream1.Read(buffer, offset, (int)_stream1.Length);
					this.Position += bytesRead;
					bytesToReadFromS2 =  count - bytesRead;
				}
				else
				{
					// will Not have to read over the boundry of the two streams
					// So read all of count from stream1 and return
					Console.WriteLine("Read 2 "); 
					bytesRead = _stream1.Read(buffer, offset, count);
					this.Position += bytesRead;
					return bytesRead;
				}
				
				// At this point we have read stream1 completly and will read the remainder
				// of count from stream2
				Console.WriteLine("Read 3 ");
				// Console.WriteLine("offset: " + offset);
				// Console.WriteLine("bytesRead: " + bytesRead);
				// Console.WriteLine("bytesToReadFromS2: " + bytesToReadFromS2);
				bytesRead += _stream2.Read(buffer, offset + bytesRead, bytesToReadFromS2);
				this.Position += bytesRead;
				return bytesRead;
			}


			  // Concat position was located in stream2 so read from stream two and return
		    // Read all of stream two and return
      	if(CanSeek)
				{
          _stream2.Position = this.Position - (int)_stream1.Length;
			    bytesRead = _stream2.Read(buffer, offset, count);

          Console.WriteLine("Read 4 bytesRead = " + bytesRead);
			    this.Position += bytesRead;			    
				}
				else
				{
					// check to see if at exact position in stream2
					//Console.WriteLine("Read: Position = " + this.Position +"");
					if(this.Position == _stream1.Length + _stream2.Position) // off by one ? 
					{
						bytesRead = _stream2.Read(buffer,offset,count);
						this.Position += bytesRead;
					}
					else
					{
						throw new NotSupportedException("Cannot write because Seek is not supported");
					}
				}
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
										
				  Console.WriteLine("2 Writing "+ bytesToWriteToS2.ToString() + " bytes to stream2");
					_stream2.Write(buffer,offsetForS2,bytesToWriteToS2);
					this.Position += bytesToWriteToS2;
					return;
				}
				else // end of stream1 will not be reached
				{
					Console.WriteLine("3 Writing "+ count.ToString() + " bytes to stream1");
					_stream1.Write(buffer,offset,count);
					this.Position += count;					
				}
			}
			else // Start writing in stream2
			{
				if(CanSeek)// seeking in stream2 requires LengthSupport
				{
						_stream2.Position = this.Position - (int)_stream1.Length;
						Console.WriteLine("4 Writing "+ count.ToString() + " bytes to stream2");
						_stream2.Write(buffer,offset,count);
            Console.WriteLine("Position += "+ count);
						this.Position += count;
				}
				else
				{
					// check to see if at exact position in stream2
					//Console.WriteLine("Write: Position = " + this.Position +"");
					if(this.Position == _stream1.Length + _stream2.Position) // off by one ? 
					{
						_stream2.Write(buffer,offset,count);
						this.Position += count;
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


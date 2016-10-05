using System;
using System.IO;

/// <summary>
/// Represents a memory stream that does not support seeking, but otherwise has
/// functionality identical to the MemoryStream class.
/// </summary>
namespace CS422
{
    public class NoSeekMemoryStream : MemoryStream
    {
        private MemoryStream _stream;
        private bool _canSeek;        
        
        //Properties
        public override bool  CanSeek
        {
            get {return _canSeek;}
        }

        public override long  Length
        {
            get 
            {
                throw new NotSupportedException(
                    "NoSeekMemoryStream: LengthNotSupported");
            }          
        }

        // Constructors
        public NoSeekMemoryStream(byte[] buffer)
        { 
            // implement
            _stream = new MemoryStream(buffer);
            _canSeek = false;
        }

        public NoSeekMemoryStream(byte[] buffer, int offset, int count)
        {
            // implement
            _stream = new MemoryStream(buffer, offset, count);
            _canSeek = false;
        }

        // Methods
        public override long Seek(long offset, SeekOrigin loc)
        {
            throw new NotSupportedException("NoSeekMemoryStream: seeking not supported.");
        }

        // public override int Read(byte[] buffer,	int offset,	int count )
        // {
        //     throw new NotImplementedException();
        // }

    }
}
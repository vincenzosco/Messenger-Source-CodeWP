namespace System.IO
{
	internal sealed class BufferedStream : Stream
	{
		private Stream m_stream;

		private byte[] m_buffer;

		private int m_buffer_pos;

		private int m_buffer_read_ahead;

		private bool m_buffer_reading;

		private bool disposed;

		public override bool CanRead
		{
			get
			{
				return m_stream.CanRead;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return m_stream.CanWrite;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return m_stream.CanSeek;
			}
		}

		public override long Length
		{
			get
			{
				Flush();
				return m_stream.Length;
			}
		}

		public override long Position
		{
			get
			{
				CheckObjectDisposedException();
				return m_stream.Position - m_buffer_read_ahead + m_buffer_pos;
			}
			set
			{
				if (value < Position && Position - value <= m_buffer_pos && m_buffer_reading)
				{
					m_buffer_pos -= (int)(Position - value);
					return;
				}
				if (value > Position && value - Position < m_buffer_read_ahead - m_buffer_pos && m_buffer_reading)
				{
					m_buffer_pos += (int)(value - Position);
					return;
				}
				Flush();
				m_stream.Position = value;
			}
		}

		public BufferedStream(Stream stream)
			: this(stream, 4096)
		{
		}

		public BufferedStream(Stream stream, int bufferSize)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", "<= 0");
			}
			if (!stream.CanRead && !stream.CanWrite)
			{
				throw new ObjectDisposedException("Cannot access a closed Stream.");
			}
			m_stream = stream;
			m_buffer = new byte[bufferSize];
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (m_buffer != null)
				{
					Flush();
				}
				m_stream.Dispose();
				m_buffer = null;
				disposed = true;
			}
		}

		public override void Flush()
		{
			CheckObjectDisposedException();
			if (m_buffer_reading)
			{
				if (CanSeek)
				{
					m_stream.Position = Position;
				}
			}
			else if (m_buffer_pos > 0)
			{
				m_stream.Write(m_buffer, 0, m_buffer_pos);
			}
			m_buffer_read_ahead = 0;
			m_buffer_pos = 0;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			CheckObjectDisposedException();
			if (!CanSeek)
			{
				throw new NotSupportedException("Non seekable stream.");
			}
			Flush();
			return m_stream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			CheckObjectDisposedException();
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value must be positive");
			}
			if (!m_stream.CanWrite && !m_stream.CanSeek)
			{
				throw new NotSupportedException("the stream cannot seek nor write.");
			}
			if (m_stream == null || (!m_stream.CanRead && !m_stream.CanWrite))
			{
				throw new IOException("the stream is not open");
			}
			m_stream.SetLength(value);
			if (Position > value)
			{
				Position = value;
			}
		}

		public override int ReadByte()
		{
			CheckObjectDisposedException();
			if (!m_stream.CanRead)
			{
				throw new NotSupportedException("Cannot read from stream");
			}
			if (!m_buffer_reading)
			{
				Flush();
				m_buffer_reading = true;
			}
			if (1 <= m_buffer_read_ahead - m_buffer_pos)
			{
				return m_buffer[m_buffer_pos++];
			}
			if (m_buffer_pos >= m_buffer_read_ahead)
			{
				m_buffer_pos = 0;
				m_buffer_read_ahead = 0;
			}
			m_buffer_read_ahead = m_stream.Read(m_buffer, 0, m_buffer.Length);
			if (1 <= m_buffer_read_ahead)
			{
				return m_buffer[m_buffer_pos++];
			}
			return -1;
		}

		public override void WriteByte(byte value)
		{
			CheckObjectDisposedException();
			if (!m_stream.CanWrite)
			{
				throw new NotSupportedException("Cannot write to stream");
			}
			if (m_buffer_reading)
			{
				Flush();
				m_buffer_reading = false;
			}
			else if (m_buffer_pos >= m_buffer.Length - 1)
			{
				Flush();
			}
			m_buffer[m_buffer_pos++] = value;
		}

		public override int Read(byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			CheckObjectDisposedException();
			if (!m_stream.CanRead)
			{
				throw new NotSupportedException("Cannot read from stream");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "< 0");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "< 0");
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException("array.Length - offset < count");
			}
			if (!m_buffer_reading)
			{
				Flush();
				m_buffer_reading = true;
			}
			if (count <= m_buffer_read_ahead - m_buffer_pos)
			{
				Buffer.BlockCopy(m_buffer, m_buffer_pos, array, offset, count);
				m_buffer_pos += count;
				if (m_buffer_pos == m_buffer_read_ahead)
				{
					m_buffer_pos = 0;
					m_buffer_read_ahead = 0;
				}
				return count;
			}
			int num = m_buffer_read_ahead - m_buffer_pos;
			Buffer.BlockCopy(m_buffer, m_buffer_pos, array, offset, num);
			m_buffer_pos = 0;
			m_buffer_read_ahead = 0;
			offset += num;
			count -= num;
			if (count >= m_buffer.Length)
			{
				num += m_stream.Read(array, offset, count);
			}
			else
			{
				m_buffer_read_ahead = m_stream.Read(m_buffer, 0, m_buffer.Length);
				if (count < m_buffer_read_ahead)
				{
					Buffer.BlockCopy(m_buffer, 0, array, offset, count);
					m_buffer_pos = count;
					num += count;
				}
				else
				{
					Buffer.BlockCopy(m_buffer, 0, array, offset, m_buffer_read_ahead);
					num += m_buffer_read_ahead;
					m_buffer_read_ahead = 0;
				}
			}
			return num;
		}

		public override void Write(byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			CheckObjectDisposedException();
			if (!m_stream.CanWrite)
			{
				throw new NotSupportedException("Cannot write to stream");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "< 0");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "< 0");
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException("array.Length - offset < count");
			}
			if (m_buffer_reading)
			{
				Flush();
				m_buffer_reading = false;
			}
			if (m_buffer_pos >= m_buffer.Length - count)
			{
				Flush();
				m_stream.Write(array, offset, count);
			}
			else
			{
				Buffer.BlockCopy(array, offset, m_buffer, m_buffer_pos, count);
				m_buffer_pos += count;
			}
		}

		private void CheckObjectDisposedException()
		{
			if (disposed)
			{
				throw new ObjectDisposedException("BufferedStream", "Stream is closed");
			}
		}
	}
}

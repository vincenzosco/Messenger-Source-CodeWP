using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Facebook.Base.Logging;
using ImageTools;
using ImageTools.IO;
using WebP;

namespace Facebook.Messenger.WP8.Utilities;

public class WebPDecoder : IImageDecoder
{
	private const int RiffHeaderSize = 12;

	private const int ChunkHeaderSize = 8;

	public int HeaderSize => 12;

	public bool IsSupportedFileExtension(string extension)
	{
		string text = extension.ToUpper(CultureInfo.CurrentCulture);
		return text == "WEBP";
	}

	public bool IsSupportedFileFormat(byte[] header)
	{
		if (header.Length >= 12)
		{
			if (header[0] == 82 && header[1] == 73 && header[2] == 70 && header[3] == 70 && header[8] == 87 && header[9] == 69 && header[10] == 66)
			{
				return header[11] == 80;
			}
			return false;
		}
		return false;
	}

	public async Task Decode(ExtendedImage image, Stream stream)
	{
		byte[] bytes = new byte[stream.Length];
		int readLength = 0;
		using (stream)
		{
			readLength = await stream.ReadAsync(bytes, 0, bytes.Length);
		}
		if (readLength < bytes.Length)
		{
			throw new Exception("Not enough data read");
		}
		WebPImage pImage = WebPImage.DecodeBytes(bytes, readLength, 1);
		if (pImage == null)
		{
			Logger.Error("Failed to decode WebP Image. Possible causes: Out of memory, unable to create iterator or demuxer.", "Decode", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8\\Utilities\\WebPDecoder.cs", 67);
			return;
		}
		WebPFrame[] frames = pImage.frames;
		foreach (WebPFrame val in frames)
		{
			if (image.Pixels == null)
			{
				image.SetPixels(val.width, val.height, val.bytes);
				image.DelayTime = pImage.frameDurationMilliseconds / 10;
				continue;
			}
			ImageFrame imageFrame = new ImageFrame();
			imageFrame.DelayTime = pImage.frameDurationMilliseconds / 10;
			imageFrame.SetPixels(val.width, val.height, val.bytes);
			image.Frames.Add(imageFrame);
		}
	}
}

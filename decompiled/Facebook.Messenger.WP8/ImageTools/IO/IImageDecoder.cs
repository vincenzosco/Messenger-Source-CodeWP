using System.IO;
using System.Threading.Tasks;

namespace ImageTools.IO;

public interface IImageDecoder
{
	int HeaderSize { get; }

	bool IsSupportedFileExtension(string extension);

	bool IsSupportedFileFormat(byte[] header);

	Task Decode(ExtendedImage image, Stream stream);
}

using System.IO;
using System.Threading.Tasks;

namespace Facebook.Base
{
	public interface IFileAccess
	{
		Task<Stream> GetFileStream(string filePath);

		Task DeleteFileAtPath(string path);

		Task<string> RenameFileAtPath(string path, string fileName);
	}
}

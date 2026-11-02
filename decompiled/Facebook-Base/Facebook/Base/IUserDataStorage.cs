using System.Threading.Tasks;

namespace Facebook.Base;

public interface IUserDataStorage
{
	Task<bool> DataExists(string key);

	Task<string> GetData(string key);

	Task SetData(string key, string data);
}

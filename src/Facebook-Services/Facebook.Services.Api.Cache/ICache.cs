using System.Threading.Tasks;

namespace Facebook.Services.Api.Cache
{
	public interface ICache
	{
		Task EnsureSave();

		Task Purge();
	}
}

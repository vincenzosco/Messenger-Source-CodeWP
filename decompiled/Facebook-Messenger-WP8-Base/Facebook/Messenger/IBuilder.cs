namespace Facebook.Messenger;

public interface IBuilder<out T>
{
	T Get();
}

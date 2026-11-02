namespace Facebook.Base
{
	public class Response
	{
		public Error Error { get; set; }

		public bool HasError()
		{
			return Error != null;
		}

		public override string ToString()
		{
			if (Error != null)
			{
				return string.Format("Title: {0}, Message: {1}, Exception: {2}", new object[3] { Error.Message, Error.Title, Error.Exception });
			}
			return "None";
		}
	}
}

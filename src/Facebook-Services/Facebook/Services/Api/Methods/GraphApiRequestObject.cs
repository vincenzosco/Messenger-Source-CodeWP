namespace Facebook.Services.Api.Methods
{
	internal class GraphApiRequestObject
	{
		public string method { get; set; }

		public string relative_url { get; set; }

		public string body { get; set; }

		public string attached_files { get; set; }

		public string name { get; set; }

		public string depends_on { get; set; }

		public bool omit_response_on_success { get; set; }

		public GraphApiRequestObject()
		{
			method = string.Empty;
			relative_url = string.Empty;
			body = string.Empty;
			attached_files = string.Empty;
			name = string.Empty;
			depends_on = string.Empty;
			omit_response_on_success = false;
		}
	}
}

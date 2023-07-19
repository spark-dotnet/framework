namespace Spark.Templates.Razor.Application.ViewModels
{
	public class ErrorViewModel
	{
		public string RequestId { get; set;}
		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}

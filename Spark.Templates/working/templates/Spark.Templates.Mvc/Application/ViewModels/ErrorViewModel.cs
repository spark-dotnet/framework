namespace Spark.Templates.Mvc.Application.ViewModels
{
	public class ErrorViewModel
	{
		public string RequestId { get; set;}
		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}
}

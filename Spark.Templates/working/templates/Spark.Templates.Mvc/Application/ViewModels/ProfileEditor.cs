namespace Spark.Templates.Mvc.Application.ViewModels
{
    public class ProfileEditor
    {
        public ProfileInfoEditor ProfileInfoEditor { get; set; } = new();
        public ProfilePasswordEditor ProfilePasswordEditor { get; set; } = new();
    }
}

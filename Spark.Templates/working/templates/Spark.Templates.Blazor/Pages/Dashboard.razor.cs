using Microsoft.AspNetCore.Components;
using Spark.Templates.Blazor.Application.Models;
using Spark.Templates.Blazor.Pages.Shared;

namespace Spark.Templates.Blazor.Pages;

public partial class Dashboard
{
    [CascadingParameter]
    public MainLayout? Layout { get; set; }
    private User? user => Layout.User;
}

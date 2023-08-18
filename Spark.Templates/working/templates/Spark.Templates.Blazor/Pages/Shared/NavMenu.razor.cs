using Microsoft.AspNetCore.Components;
using Spark.Templates.Blazor.Application.Models;

namespace Spark.Templates.Blazor.Pages.Shared;

public partial class NavMenu
{
    [CascadingParameter]
    public MainLayout? Layout { get; set; }
    private User? user => Layout?.User;
}

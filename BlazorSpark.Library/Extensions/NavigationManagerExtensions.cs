using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Extensions
{
    public static class NavigationManagerExtensions
    {
        public static string Route(this NavigationManager navManager)
        {
            return navManager.ToBaseRelativePath(navManager.Uri);
        }
    }
}

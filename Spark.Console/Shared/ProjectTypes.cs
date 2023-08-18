using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Console.Shared;

public class ProjectTypes
{
    public const string mvc = nameof(mvc);
    public const string blazor = nameof(blazor);
    public const string razor = nameof(razor);

		public static bool IsValid(string template)
        {
            if (template == mvc) return true;
            if (template == blazor) return true;
            if (template == razor) return true;
			if (template == api) return true;
			return false;
        }
        public static string TranslateProjectTypeToTemplate(string projectType)
        {
            if (projectType == mvc) return "sparkmvc";
            if (projectType == blazor) return "sparkblazor";
            if (projectType == razor) return "sparkrazor";
			if (projectType == api) return "sparkapi";
			return "sparkblazor";
        }
    }
}

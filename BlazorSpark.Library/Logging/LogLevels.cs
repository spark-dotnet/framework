using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Logging
{
    internal class LogLevels
    {
        public const string debug = nameof(debug);
        public const string information = nameof(information);
        public const string warning = nameof(warning);
        public const string error = nameof(error);
        public const string fatal = nameof(fatal);
    }
}

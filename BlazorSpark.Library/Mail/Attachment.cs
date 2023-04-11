using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSpark.Library.Mail
{
    public class Attachment
    {
        public byte[] Bytes { get; set; }
        public string Name { get; set; }
    }
}

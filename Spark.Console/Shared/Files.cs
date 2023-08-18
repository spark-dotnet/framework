using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Console.Shared;

public class Files
{
    public static bool WriteFileIfNotCreatedYet(string path, string filename, string content)
    {
        string fullFilePath = path + "/" + filename;

        Directory.CreateDirectory(path);

        if (!File.Exists(fullFilePath))
        {
            using (var file = File.CreateText(fullFilePath))
            {
                file.Write(content);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}

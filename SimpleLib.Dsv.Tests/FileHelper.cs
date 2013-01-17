using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimpleLibTest
{
    public class FileHelper
    {
        /// <summary>
        /// Be very careful with how this is used, it will only work properly if this assembly is located
        /// on the same folder as the running application (so don't add it to the GAC for example)
        /// </summary>
        /// <returns></returns>
        public static string GetProgramFolder()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar; 
        }
    }
}

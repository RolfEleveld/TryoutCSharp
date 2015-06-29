using System;
using System.IO;

namespace PseudoLocalizer
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = args.Length > 0 ? args[0] : string.Empty;
            if (File.Exists(filename))
            {
                string translatedFile = Localizer.Localize(filename, "zh-CN");
                Console.Out.WriteLine(translatedFile);
            }
            else
            {
                Console.Error.WriteLine(string.Format("Usage: {0} {1}", "PseudoLocalizer", "File To translate"));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace VPNBackupReasurrance
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Backup directory validation is in progress...");
            
            string ConfigPath = ApplicationPath + @"\configuration.xml";
            Configuration appConfig = new Configuration(ConfigPath);

            Logging LogFile = new Logging(appConfig.Logfile);

            DirectoryDiff Diff = new DirectoryDiff();
            Diff.Log = LogFile;

            foreach (Location check in appConfig.Locations)
            {
                try
                {
                    List<string> results = Diff.FindDifferences(check.Name, check.Source, check.Backup);
                }

                catch (Exception e)
                {
                    LogFile.storeError(e);
                }
            }

            LogFile.CloseLog();
        }

        static string ApplicationPath
        {
            get { return Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory); }
        }
    }
}

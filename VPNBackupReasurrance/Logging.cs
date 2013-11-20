using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VPNBackupReasurrance
{
    public class Logging
    {
        public Logging(string logPath)
        {
            // Create the error file
            BaseFilePath = logPath;
            ErrorLog = CreateNewErrorFile(BaseFilePath);
        }

        private string _baseFilePath;
        public string BaseFilePath
        {
            get { return _baseFilePath; }
            set { _baseFilePath = value; }
        }

        private StreamWriter _errorLog;
        public StreamWriter ErrorLog
        {
            get  { return _errorLog; }
            private set { _errorLog = value; }
        }

        private List<string> _fileDiffs;
        private List<string> FileDiffs
        {
            get 
            { 
                if (_fileDiffs == null) 
                {
                    _fileDiffs = new List<String>();
                }
                return _fileDiffs;
            }
        }

        public void storeFileDiff(string diff)
        {
            FileDiffs.Add(diff);
        }
        
        private List<Exception> _errors;
        private List<Exception> Errors
        {
            get
            {
                if (_errors == null)
                {
                    _errors = new List<Exception>();
                }
                return _errors;
            }
        }

        public void storeError(Exception error)
        {
            Errors.Add(error);
        }

        private string _start;

        private StreamWriter CreateNewErrorFile(string pBaseFilePath)
        {
            _start = DateTime.Now.ToString();

            string dt = DateTime.Now.ToShortDateString()
                .Replace(@"/",@"").Replace(@"\",@"");
            string tm = DateTime.Now.ToLongTimeString()
                .Replace(@":", @"").Replace(@" ", @"");

            StringBuilder logFile = new StringBuilder();
            logFile.Append(dt).Append(@"_").Append(tm).Append(@".log");

            FileStream fs = new FileStream(pBaseFilePath + logFile.ToString(),
                FileMode.CreateNew, FileAccess.Write, FileShare.None);

            StreamWriter swFromFileStream = new StreamWriter(fs);

            return swFromFileStream;
        }

        public void CloseLog()
        {
            if (ErrorLog != null)
            {
                WriteLog();
                ErrorLog.Flush();
                ErrorLog.Close();
                ErrorLog = null;
            }
        }

        public void WriteLog()
        {
            string errorHeader = "Errors encountered while processing: \r\n";
            string differenceHeader = "Backups that exist on main server but not backup server: \r\n";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(_start);

            sb.AppendLine("");
            sb.AppendLine("");

            sb.AppendLine(differenceHeader);
            foreach (string file in FileDiffs)
            {
                sb.Append(file);
                sb.AppendLine("\r\n");
            }

            sb.AppendLine("");
            sb.AppendLine("");

            sb.AppendLine(errorHeader);
            foreach (Exception e in Errors)
            {
                sb.Append(e.ToString());
                sb.AppendLine("\r\n");
            }

            sb.AppendLine("");
            sb.AppendLine("");

            sb.AppendLine(DateTime.Now.ToString());

            ErrorLog.WriteLine(sb.ToString());
        }
    }
}

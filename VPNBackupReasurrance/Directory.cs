using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VPNBackupReasurrance
{
    public class Directory
    {
        private string _sourceOrBackup;
        public string SourceOrBackup
        {
            get { return _sourceOrBackup; }
            set { _sourceOrBackup = value; }
        }

        private bool _mappedDrive;
        public bool MappedDrive
        {
            get { return _mappedDrive; }
            set { _mappedDrive = value; }
        }

        private string _shareName;
        public string ShareName
        {
            get { return _shareName; }
            set { _shareName = value; }
        }

        private string _localDrive;
        public string LocalDrive
        {
            get { return _localDrive; }
            set { _localDrive = value; }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(SourceOrBackup + ": ");
            sb.AppendLine("\tSharename: " + ShareName);
            sb.AppendLine("\tLocalPath: " + LocalDrive);
            sb.AppendLine("\tUsername: " + Username);
            sb.AppendLine("\tPassword: " + Password);
            sb.AppendLine("");
            return sb.ToString();
        }
    }
}

using System;

namespace VPNBackupReasurrance
{
    public class Location
    {
        public Location()
        {
        }

        private string _source;
        private string _backup;

        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public string Backup
        {
            get { return _backup; }
            set { _backup = value; }
        }

    }
}

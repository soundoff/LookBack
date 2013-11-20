using System;
using System.Text;

namespace VPNBackupReasurrance
{
    public class Location
    {
        public Location()
        {
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private Directory _source;
        private Directory _backup;

        public Directory Source
        {
            get 
            {
                if (_source == null)
                {
                    _source = new Directory();
                }
                return _source; 
            }
            set { _source = value; }
        }

        public Directory Backup
        {
            get
            {
                if (_backup == null)
                {
                    _backup = new Directory();
                }
                return _backup;
            }
            set { _backup = value; }
        }
    }
}

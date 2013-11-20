using System;
using System.Text;

namespace VPNBackupReasurrance
{
    class DiffException : System.Exception
    {
        private string locationName;

        public string LocationName
        {
            get { return locationName; }
            set { locationName = value; }
        }

        private Directory errorLocation;

        public Directory ErrorLocation
        {
            get 
            {
                if (errorLocation == null)
                {
                    errorLocation = new Directory();
                } 
                return errorLocation;
            }
            set { errorLocation = value; }
        }

        private Exception _thrownException;

        public Exception ThrownException
        {
            get { return _thrownException; }
            set { _thrownException = value; }
        }


        public override string ToString()
        {
            return ("Location name: " + this.LocationName + "\r\n" + this.ThrownException.ToString());
        }
    }
}

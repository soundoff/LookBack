using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VPNBackupReasurrance
{
    // This implementation defines a very simple comparison
    // between two FileInfo objects. It only compares the name
    // of the files being compared and their length in bytes.
    class FileCompare : IEqualityComparer<FileInfo>
    {
        public FileCompare() { }

        public bool Equals(FileInfo f1, FileInfo f2)
        {
            return (GetHashCode(f1) == GetHashCode(f2));
        }

        // Return a hash that reflects the comparison criteria. According to the 
        // rules for IEqualityComparer<T>, if Equals is true, then the hash codes must
        // also be equal. Because equality as defined here is a simple value equality, not
        // reference identity, it is possible that two or more objects will produce the same
        // hash code.
        public int GetHashCode(FileInfo fi)
        {
            try
            {
                string s = String.Format("{0}{1}", fi.Name, fi.Length);
                return s.GetHashCode();
            }
            catch (FileNotFoundException ex)
            {
                return 0;
            }

            catch (DirectoryNotFoundException ex)
            {
                return 0;
            }
        }
    }
}

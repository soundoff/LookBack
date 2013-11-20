using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using aejw.Network;

namespace VPNBackupReasurrance
{
    class DirectoryDiff
    {
        private Logging _log;
        public Logging Log
        {
            get { return _log; }
            set { _log = value; }
        }

        public List<string> FindDifferences(string name, Directory source, Directory backup)
        {
            string SourcePath = source.LocalDrive.Trim();
            string BackupPath = backup.LocalDrive.Trim();

            // Create a network mapped drive
            NetworkDrive sourcedrive = new NetworkDrive();
            // Create a network mapped drive
            NetworkDrive backupdrive = new NetworkDrive();


            try
            {
                if (source.MappedDrive)
                {
                    sourcedrive.ShareName = source.ShareName.Trim();
                    sourcedrive.LocalDrive = source.LocalDrive.Trim();
                    sourcedrive.Force = true;
                    
                    sourcedrive.MapDrive(source.Username.Trim(), source.Password.Trim());
                    
                    StringBuilder sb = new StringBuilder();
                    if (sourcedrive.LocalDrive == "")
                        sb.Append(sourcedrive.ShareName);
                    else
                        sb.Append(sourcedrive.LocalDrive);

                    SourcePath = sb.ToString();
                }
                
                if (backup.MappedDrive)
                {
                    backupdrive.ShareName = backup.ShareName.Trim();
                    backupdrive.LocalDrive = backup.LocalDrive.Trim();
                    backupdrive.Force = true;

                    backupdrive.MapDrive(backup.Username.Trim(), backup.Password.Trim());
                    
                    StringBuilder sb = new StringBuilder();
                    if (backupdrive.LocalDrive == "")
                        sb.Append(backupdrive.ShareName);
                    else
                        sb.Append(backupdrive.LocalDrive);

                    BackupPath = sb.ToString();
                }
                
                // Create two identical or different temporary folders 
                // on a local drive and change these file paths.
                DirectoryInfo SourceDir = new DirectoryInfo(SourcePath);
                DirectoryInfo BackupDir = new DirectoryInfo(BackupPath);
            
                // Take a snapshot of the file system.
                IEnumerable<FileInfo> SourceList = SourceDir.GetFiles("*.*", SearchOption.AllDirectories);
                IEnumerable<FileInfo> BackupList = BackupDir.GetFiles("*.*", SearchOption.AllDirectories);

                //A custom file comparer defined below
                FileCompare myFileCompare = new FileCompare();

                // This query determines whether the two folders contain
                // identical file lists, based on the custom file comparer
                // that is defined in the FileCompare class.
                // The query executes immediately because it returns a bool.
                bool areIdentical = SourceList.SequenceEqual(BackupList, myFileCompare);

                List<string> results = new List<string>();
                if (!areIdentical == true)
                {
                    // Find the set difference between the two folders.
                    // For this example we only check one way.
                    var sourceListOnly = (from file in SourceList
                                          select file).Except(BackupList, myFileCompare);

                    foreach (FileInfo file in sourceListOnly)
                    {
                        string missingFile = file.DirectoryName + @"\" + file.Name;
                        if (File.Exists(missingFile))
                        {
                            results.Add(missingFile);
                        }
                    }
                }

                if (results.Count > 0)
                {
                    foreach (string result in results)
                    {
                        Log.storeFileDiff(result);
                    }
                }

                if (source.MappedDrive)
                {
                    sourcedrive.UnMapDrive();
                }

                if (backup.MappedDrive)
                {
                    backupdrive.UnMapDrive();
                }

                return results;
            }

            catch (Exception ex)
            {
                DiffException dex = new DiffException();
                dex.LocationName = name;
                dex.ErrorLocation = backup;
                dex.ThrownException = ex;
                Log.storeError(dex);

                return null;
            }
        }
    }
}

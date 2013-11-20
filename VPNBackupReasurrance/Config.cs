using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace VPNBackupReasurrance
{
    public class Configuration
    {
        public Configuration(string pConfigFile)
        {
            ConfigFileLocation = pConfigFile;
            LoadConfiguration();
        }

        private string _configFileLocation;
        public string ConfigFileLocation
        {
            get { return _configFileLocation; }
            set { _configFileLocation = value; }
        }

        private List<Location> _locations;
        public List<Location> Locations
        {
            get 
            {
                if (_locations == null)
                    _locations = new List<Location>();
                return _locations; 
            }
            set { _locations = value; }
        }

        private string _logfile;
        public string Logfile
        {
            get { return _logfile; }
            set { _logfile = value; }
        }

        public void LoadConfiguration()
        {
            // Create the XDocument object by loading the config file
            XDocument config = XDocument.Load(ConfigFileLocation);

            // Next get the types of documents to process this session
            LoadDirectories(config).LoadLogPath(config);
        }

        private Configuration LoadDirectories(XDocument config)
        {
            var locations = config.Root
                                  .Elements("locations")
                                  .Descendants("location")
                                  .Select(path => new
                                  {
                                     Name = path.Element("name").Value,
                                     Source = path.Element("source"),
                                     Backup = path.Element("backup")
                                  });

            foreach (var configLocation in locations)
            {
                Location location = new Location();
                location.Name = configLocation.Name;
                location.Source.SourceOrBackup = "Source";
                location.Source.MappedDrive = 
                    Convert.ToBoolean(configLocation.Source.Element("mapthisdrive").Value);
                location.Source.ShareName = configLocation.Source.Element("sharename").Value;
                location.Source.LocalDrive = configLocation.Source.Element("localdrive").Value;
                location.Source.Username = configLocation.Source.Element("username").Value;
                location.Source.Password = configLocation.Source.Element("password").Value;

                location.Backup.SourceOrBackup = "Backup";
                location.Backup.MappedDrive = 
                    Convert.ToBoolean(configLocation.Backup.Element("mapthisdrive").Value);
                location.Backup.ShareName = configLocation.Backup.Element("sharename").Value;
                location.Backup.LocalDrive = configLocation.Backup.Element("localdrive").Value;
                location.Backup.Username = configLocation.Backup.Element("username").Value;
                location.Backup.Password = configLocation.Backup.Element("password").Value;

                Locations.Add(location);
            }
            return this;
        }

        private Configuration LoadLogPath(XDocument config)
        {
            var path = config.Root
                                    .Element("logfile")
                                    .Value;

            Logfile = path.ToString();
            return this;
        }
    }
}

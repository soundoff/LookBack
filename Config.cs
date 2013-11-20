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

        public void LoadConfiguration()
        {
            // Create the XDocument object by loading the config file
            XDocument config = XDocument.Load(ConfigFileLocation);

            // Next get the types of documents to process this session
            LoadDirectories(config);
        }

        private void LoadDirectories(XDocument config)
        {
            var documentTypes = config.Root
                                      .Elements("TypesOfDocuments")
                                      .Descendants("Type")
                                      .Select(rule => new
                                      {
                                          Description = rule.Element("Description").Value,
                                          Start = rule.Element("HowToDetermine").Element("Start").Value,
                                          End = rule.Element("HowToDetermine").Element("End").Value,
                                          Split = rule.Element("SplitMe").Value,
                                          Rename = rule.Element("FileNaming").Element("Rename").Value,
                                          NameParts = rule.Element("FileNaming").Element("NameParts").Value,
                                          Extension = rule.Element("FileNaming").Element("Extension").Value
                                      });

            foreach (var doctype in documentTypes)
            {
                StartEnd howToFind = new StartEnd();
                howToFind.FindStart = doctype.Start;
                howToFind.FindEnd = doctype.End;

                DocumentType docType = new DocumentType();
                docType.Description = doctype.Description;
                docType.HowToEvalType = howToFind;
                docType.SplitMe = Convert.ToBoolean(doctype.Split);
                docType.Rename = Convert.ToBoolean(doctype.Rename);
                docType.NewFileNameParts = doctype.NameParts;
                docType.NewFileExtension = doctype.Extension;
                DocumentTypes.Add(docType);
            }
        }
    }
}

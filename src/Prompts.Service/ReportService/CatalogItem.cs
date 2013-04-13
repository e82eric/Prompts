using System;
using System.Xml.Serialization;

namespace Prompts.Service.ReportService
{
    public class CatalogItem
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string VirtualPath { get; set; }
        public ItemTypeEnum Type { get; set; }
        public int Size { get; set; }

        [XmlIgnoreAttribute]
        public bool SizeSpecified { get; set; }

        public string Description { get; set; }
        public bool Hidden { get; set; }

        [XmlIgnoreAttribute]
        public bool HiddenSpecified { get; set; }

        public DateTime CreationDate { get; set; }

        [XmlIgnore]
        public bool CreationDateSpecified { get; set; }

        public DateTime ModifiedDate { get; set; }

        [XmlIgnoreAttribute]
        public bool ModifiedDateSpecified { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string MimeType { get; set; }

        public DateTime ExecutionDate { get; set; }

        [XmlIgnoreAttribute]
        public bool ExecutionDateSpecified { get; set; }
    }
}
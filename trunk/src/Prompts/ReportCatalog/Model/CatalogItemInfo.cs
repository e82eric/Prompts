using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Prompts.ReportCatalog.Model
{
    public class CatalogItemInfo
    {
        public ObservableCollection<CatalogItemInfo> Children { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public CatalogItemType Type { get; set; }
    }
}
using Prompts.ReportCatalog.Model;

namespace Test.Prompts.Infrastructure.Builders
{
    public class CatalogItemInfoBuilder
    {
        private string _name = "Name";
        private string _path = "Path";
        private CatalogItemType _type = CatalogItemType.Report;

        public CatalogItemInfoBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CatalogItemInfoBuilder WithPath(string path)
        {
            _path = path;
            return this;
        }

        public CatalogItemInfoBuilder WithType(CatalogItemType type)
        {
            _type = type;
            return this;
        }

        public CatalogItemInfo Build()
        {
            return new CatalogItemInfo {Name = _name, Path = _path, Type = _type};
        }
    }
}

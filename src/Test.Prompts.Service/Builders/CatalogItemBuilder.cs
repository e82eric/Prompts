using Prompts.Service.ReportService;

namespace Test.Prompts.Service.Builders
{
    public class CatalogItemBuilder
    {
        private string _name = "Name";
        private string _path = "Path";
        private ItemTypeEnum _type = ItemTypeEnum.Unknown;
        private bool _hidden = false;

        public CatalogItemBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public CatalogItemBuilder WithPath(string path)
        {
            _path = path;
            return this;
        }

        public CatalogItemBuilder WithType(ItemTypeEnum type)
        {
            _type = type;
            return this;
        }

        public CatalogItemBuilder WithHiddenFlag(bool hidden)
        {
            _hidden = hidden;
            return this;
        }

        public CatalogItem Build()
        {
            return new CatalogItem {Name = _name, Path = _path, Type = _type, Hidden = _hidden};
        }
    }
}

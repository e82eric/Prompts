namespace Test.Prompts.Service.Infastructure
{
    public static class EntityExtensions
    {
        public static T[] ToArray<T>(this T source)
        {
            return new[]{source};
        }
    }
}

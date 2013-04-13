using System;

namespace Test.Prompts.Service.Infastructure
{
    class ValueObjectBuilder<T> where T:new()
    {
        private readonly T _defaultT;

        public ValueObjectBuilder()
        {
            _defaultT = new T();
        }

        public T With(params Action<T>[] actions)
        {
            foreach (var action in actions)
            {
                action(_defaultT);
            }
            return _defaultT;
        }
    }
}

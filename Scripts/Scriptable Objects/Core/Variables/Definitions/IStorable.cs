namespace Toolbox.ScriptableObjects.Variables
{
    public interface IStorable<T>
    {
        void Save();
        T Load();
    }
}
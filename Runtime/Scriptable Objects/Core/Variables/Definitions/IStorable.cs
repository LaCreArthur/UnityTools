namespace AS.Toolbox.ScriptableObjects
{
    public interface IStorable<T>
    {
        void Save();
        T Load();
    }
}

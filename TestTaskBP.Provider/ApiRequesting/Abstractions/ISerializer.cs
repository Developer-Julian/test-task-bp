namespace TestTaskBP.Provider.ApiRequesting.Abstractions
{
    public interface ISerializer
    {
        T Deserialize<T>(byte[] serialized);
        byte[] Serialize<T>(T obj);
    }
}
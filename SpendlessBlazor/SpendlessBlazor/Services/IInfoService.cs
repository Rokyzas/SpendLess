using SpendlessBlazor.Shared;

namespace SpendlessBlazor.Services
{
    public interface IInfoService <T>
    {
        List<T> ReadJson();

    }
}

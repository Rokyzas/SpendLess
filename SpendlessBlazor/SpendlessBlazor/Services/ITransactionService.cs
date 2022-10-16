using SpendlessBlazor.Data;

namespace SpendlessBlazor.Services
{
    public interface ITransactionService <T>
    {
        List<T> ReadJson();

    }
}

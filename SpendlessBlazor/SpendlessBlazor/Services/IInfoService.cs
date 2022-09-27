using SpendlessBlazor.Shared;

namespace SpendlessBlazor.Services
{
    public interface IInfoService
    {
        List<Info> readJson();

        void writeToJson(Shared.Info info);

    }
}

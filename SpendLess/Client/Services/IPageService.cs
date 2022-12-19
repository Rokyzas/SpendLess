namespace SpendLess.Client.Services
{
    public interface IPageService
    {
        Task<IEnumerable<string>> Search(string value);
        List<DateTime> GetDates(DateTime date);
        Task DeleteRow(int id);
    }
}

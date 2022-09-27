namespace SpendlessBlazor.Shared
{
    public class Info
    {
        public int? expenseValue { get; set; }

        public string? categoryValue { get; set; }

        public int? dateValue { get; set; }

        public Info(int? expenseValue, string? categoryValue, int? dateValue)
        {
            this.expenseValue = expenseValue;
            this.categoryValue = categoryValue;
            this.dateValue = dateValue;
        }
    }
}

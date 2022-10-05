namespace SpendlessBlazor.Shared
{
    public class Info
    {
        public DateTime? date { get; set; } = DateTime.Today;
        public string? textValue { get; set; }
        public double amount { get; set; }

        public string? categoryValue { get; set; }

        public Info(string? textValue, double amount, string? categoryValue, DateTime? date)
        {
            this.textValue = textValue;
            this.amount = amount;
            this.categoryValue = categoryValue;
            this.date = date;
        }
    }
}

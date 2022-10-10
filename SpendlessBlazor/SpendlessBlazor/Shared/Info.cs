namespace SpendlessBlazor.Shared
{
    public class Info
    {
        public DateTime? date { get; set; } = DateTime.Today;
        public string? textValue { get; set; }
        public double amount { get; set; }

        public int? categoryValue { get; set; }

        public Info(string? textValue, double amount, int? categoryValue, DateTime? date)
        {
            this.textValue = textValue;
            this.amount = amount;
            this.categoryValue = categoryValue;
            this.date = date;
        }
    }
    public enum categoryValues
    {
        Housing, Transportation, Food, Utilities, Investing,
        Household, Personal, PersonalDevelopment, Gifts_Or_Donations,
        Entertainment, Medical_Or_Healthcare, Insurance, Kids,
        Pets, Subscriptions, Clothing, Travel, Technology
    }
}

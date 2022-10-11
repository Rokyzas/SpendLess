namespace SpendlessBlazor.Shared
{
    public class Info
    {
        public int? elementID { get; set;}
        public string? textValue { get; set;}
        public double amount { get; set; }

        public CategoryValues categoryValue { get; set; }
        public DateTime? date { get; set; } = DateTime.Today;



        public Info(int? elementID, string? textValue, double amount, CategoryValues categoryValue, DateTime? date)
        {
            this.elementID = elementID;
            this.textValue = textValue;
            this.amount = amount;
            this.categoryValue = categoryValue;
            this.date = date;
        }
    }
    public enum CategoryValues
    {
        Housing, Transportation, Food, Utilities, Investing,
        Household, PersonalDevelopment, Gifts,
        Entertainment, Healthcare, Insurance, Kids,
        Pets, Subscriptions, Clothing, Travel, Technology
    }
}

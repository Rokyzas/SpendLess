namespace SpendlessBlazor.Data
{
    public struct Transaction
    {
        public int? elementID { get; set; }
        public string? textValue { get; set; }
        public double amount { get {return amount;} set { amount = Math.Floor(100 * value) / 100;} }
        public CategoryValues categoryValue { get; set; }
        public DateTime? date { get; set; } = DateTime.Today; 

        public Transaction() { }

    }

    public enum CategoryValues
    {
        Income, Housing, Transportation, Food, Utilities, Investing,
        Household, PersonalDevelopment, Gifts,
        Entertainment, Healthcare, Insurance, Kids,
        Pets, Subscriptions, Clothing, Travel, Technology
    }
}

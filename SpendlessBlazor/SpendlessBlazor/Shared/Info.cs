namespace SpendlessBlazor.Shared
{
    public class Info
    {
        public int? elementID { get; set;}
        public string? textValue { get; set;}
        public double amount { get; set; }
        public string? categoryValue { get; set; }
        public DateTime? date { get; set; } = DateTime.Today;

        public void Delete()
        {

        }


        public Info(int? elementID, string? textValue, double amount, string? categoryValue, DateTime? date)
        {
            this.elementID = elementID;
            this.textValue = textValue;
            this.amount = amount;
            this.categoryValue = categoryValue;
            this.date = date;
        }
    }
}

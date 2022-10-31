namespace SpendLess.Shared
{
    public class Transaction : IComparable<Transaction>
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public string Category { get; set; }

        private double amount;
        public double? Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                amount = Math.Round((value ?? 0), 2);
            }
        }
        public DateTime Date { get; set; }

        public Transaction(int Id, double? amount, string category, DateTime date, string comment = "Transaction")
        {
            this.Id = Id;
            this.Comment = comment;
            this.Amount = amount;
            this.Category = category;
            this.Date = date;
        }

        public int CompareTo(Transaction x)
        {
            if (x.Date < this.Date)
            {
                return -1;
            }
            else if (x.Date > this.Date)
            {
                return 1;
            }
            else
                return 0;

            throw new NotImplementedException();
        }
    }


    public enum CategoryValues
    {
        Income, Housing, Transportation, Food, Utilities, Investing,
        Household, Personal, Gifts,
        Entertainment, Healthcare, Insurance, Kids,
        Pets, Subscriptions, Clothing, Travel, Technology
    }
}
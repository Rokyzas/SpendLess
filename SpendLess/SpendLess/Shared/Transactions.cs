namespace SpendLess.Shared
{
    public struct Transaction : IComparable<Transaction>
    {
        private int elementID;
        private string? textValue;
        private double amount;
        private CategoryValues categoryValue;
        public DateTime? date { get; set; } = DateTime.Today;

        public Transaction(int elementID, double? amount, CategoryValues categoryValue, DateTime? dateTime, string textValue = "Transaction") : this()
        {
            this.elementID = elementID;
            this.TextValue = textValue;
            this.Amount = amount;
            this.CategoryValue = categoryValue;
            this.date = dateTime;
        }

        // Properties
        public double? Amount
        {
            get { return this.amount; }
            set
            {
                this.amount = Math.Round((value ?? 0), 2);
            }
        }

        public string? TextValue
        {
            get
            {
                return this.textValue;
            }

            set
            {
                this.textValue = value;
            }
        }
        public CategoryValues CategoryValue
        {
            get
            {
                return this.categoryValue;
            }

            set
            {
                this.categoryValue = value;
            }
        }
        public int ElementID { get; set; }

        public int CompareTo(Transaction x)
        {
            if (x.date < this.date)
            {
                return -1;
            }
            else if (x.date > this.date)
            {
                return 1;
            }
            else
                return 0;

            throw new NotImplementedException();
        }
    }


    [Flags]
    public enum CategoryValues
    {
        Income, Housing, Transportation, Food, Utilities, Investing,
        Household, Personal, Gifts,
        Entertainment, Healthcare, Insurance, Kids,
        Pets, Subscriptions, Clothing, Travel, Technology
    }
}
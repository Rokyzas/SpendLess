using SpendlessBlazor.Services;
using static MudBlazor.Colors;

namespace SpendlessBlazor.Data
{
    public struct Transaction
    {
        private int elementID;
        private string? textValue;
        private double amount;
        private CategoryValues categoryValue;
        public DateTime? date { get; set; } = DateTime.Today;
        public Transaction(int elementID, string textValue, double amount, CategoryValues categoryValue, DateTime dateTime) : this()
        {
            this.elementID = elementID;
            this.TextValue = textValue;
            this.Amount = amount;
            this.CategoryValue = categoryValue;
            this.date = dateTime;
        }

        // Properties
        public double Amount
        {
            get { return this.amount; }
            set { this.amount = value; }
        }

        public string TextValue
        {
            get
            {
                if (this.textValue != null)
                    return this.textValue;
                else
                {   
                    return "Invalid item!";
                }
                    
            }

            set
            {
                if (value != null)
                    this.textValue = value;
                else
                {
                    SnackBarService.WarningMsg("Item field is empty!");
                }
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
                if (value <= Enum.GetValues(typeof(CategoryValues)).Cast<CategoryValues>().Max())
                    this.categoryValue = value;

                else
                {
                    SnackBarService.WarningMsg("Wrong category value!");
                }
                    

            }
        }
        public int ElementID
        {
            get { return this.elementID; }
            set { elementID = value; }
        }
    }

    public enum CategoryValues
    {
        Income, Housing, Transportation, Food, Utilities, Investing,
        Household, PersonalDevelopment, Gifts,
        Entertainment, Healthcare, Insurance, Kids,
        Pets, Subscriptions, Clothing, Travel, Technology
    }
}
